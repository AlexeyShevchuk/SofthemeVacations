using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Vacations.DAL.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Vacations.BLL.Models;

namespace Vacations.BLL.Services
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public UsersService(
            UserManager<User> userManager,
            IConfiguration configuration,
            RoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IMapper mapper
        )
        {
            _emailSender = emailSender;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<TokenDto> GetTokenAsync(string authorizationHeader)
        {
            if (authorizationHeader.StartsWith("Basic"))
            {
                var credValue = authorizationHeader.Substring("Basic ".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var userEmailAndPass = usernameAndPassenc.Split(":");

                var result = await _signInManager.PasswordSignInAsync(userEmailAndPass[0], userEmailAndPass[1], false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == userEmailAndPass[0]);

                    var tokenDto = new TokenDto(
                        await GenerateJwtTokenAsync(userEmailAndPass[0], appUser),
                        (await _userManager.GetRolesAsync(appUser)).FirstOrDefault());

                    return tokenDto;
                }
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        public async Task<string> GenerateJwtTokenAsync(string email, User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(30));

            var token = new JwtSecurityToken(
                _configuration["Token:Issuer"],
                _configuration["Token:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task ResetPasswordAsync(string employeeId, string code, string passwordReset)
        {
            var userEntity = await _userManager.FindByIdAsync(employeeId);
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(code);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            await _userManager.ResetPasswordAsync(userEntity, codeDecoded, passwordReset);
        }

        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
            var callbackUrl =
                $"{_configuration["Domain:RequestScheme"]}://{_configuration["Domain:DomainName"]}/auth/reset-password?id={user.Id}&code={codeEncoded}";
            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        }

        public async Task<string> GetUserRole(User user)
        {
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task<string> GetUserRoleId(User user)
        {
            foreach (var role in _roleManager.Roles)
            {
                if(role.Name.Equals(await GetUserRole(user)))
                {
                    return role.Id;
                }
            }

            return null;
        }

        public async Task UpdateUserRole(User user, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role != null)
            {
                var oldRole = await GetUserRole(user);

                if (oldRole != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRole);
                }

                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }

        public async Task SetUserRole(User user, string roleId)
        {
            var result4 = await _roleManager.FindByIdAsync(roleId);

            foreach (var role in _roleManager.Roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            if (result4 != null)
            {
                await _userManager.AddToRoleAsync(user, result4.Name);
            }
        }

        public async Task UpdateUser(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task CreateAsync(User user, string password)
        {
            await _userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            var roles = _roleManager.Roles;

            return _mapper.Map<IEnumerable<Role>, IEnumerable<RoleDto>>(roles);
        }
    }
}
