
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Vacations.BLL.Services;
using Vacations.DAL.Models;

namespace Vacations.API.Controllers
{
    public class RoleAndToken
    {
        public string Token { get; }
        public object Roles { get; }

        public RoleAndToken(string token, object roles)
        {
            Token = token;
            Roles = roles;
        }
    }


    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private IEmailSender _emailSender;
        private AccountsDbContext _context;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            AccountsDbContext context,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender
            )
        {
            _emailSender = emailSender;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<IActionResult> GetTokenAsync()
        {
            var header = Request.Headers["Authorization"];

            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var userEmailAndPass = usernameAndPassenc.Split(":");

                var result = await _signInManager.PasswordSignInAsync(userEmailAndPass[0], userEmailAndPass[1], false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == userEmailAndPass[0]);

                    var roleAndToken = new RoleAndToken(await GenerateJwtTokenAsync(userEmailAndPass[0], appUser), await _userManager.GetRolesAsync(appUser));

                    return Ok(roleAndToken);
                }
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public async Task<IActionResult> RegisterAsync()
        {
            var header = Request.Headers["Authorization"];

            //AddRole("Admin");
            //AddRole("TeamLead");
            //AddRole("User");
                         
            if (header.ToString().StartsWith("Basic"))
            {
                var credValue = header.ToString().Substring("Basic ".Length).Trim();
                var usernameAndPassenc = Encoding.UTF8.GetString(Convert.FromBase64String(credValue));
                var userEmailAndPass = usernameAndPassenc.Split(":");
                var model = new RegisterDto() { Email = userEmailAndPass[0], Password = userEmailAndPass[1] };
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmployeeId = Guid.Parse("2048e440-d225-4570-b58d-4e4d42e646d9")
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                var result1 = await _userManager.AddToRoleAsync(user, "Admin");

                //if (result.Succeeded)
                //{
                await _signInManager.SignInAsync(user, false);
                    return Ok(GenerateJwtTokenAsync(model.Email, user));
                //}
            }
            throw new ApplicationException("UNKNOWN_ERROR");
        }

        public class ForgotPasswordViewModel
        {
            public string Email { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                // If user has to activate his email to confirm his account, the use code listing below
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                //{
                //    return Ok();
                //}
                if (user == null)
                {
                    return Ok();
                }
                
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return Ok();
            }

            // If we got this far, something failed, redisplay form
            return BadRequest(ModelState);
        }

        //[HttpPut]
        //[AllowAnonymous]
        //[Route("api/password/{email}")]
        //public async Task<IActionResult> SendPasswordEmailResetRequestAsync(string email, [FromBody] PasswordReset passwordReset)
        //{
        //    //some irrelevant validatoins here
        //    await _myIdentityWrapperService.ResetPasswordAsync(email, passwordReset.Password, passwordReset.Code);
        //    return Ok();
        //}

        ////in MyIdentityWrapperService
        //public async Task ResetPasswordAsync(string email, string password, string code)
        //{
        //    var userEntity = await _userManager.FindByNameAsync(email);
        //    var codeDecodedBytes = WebEncoders.Base64UrlDecode(code);
        //    var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
        //    await _userManager.ResetPasswordAsync(userEntity, codeDecoded, password);
        //}

        public async Task<IActionResult> AddRole(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
            return Json(_roleManager.Roles);
        }

        private async Task<string> GenerateJwtTokenAsync(string email, User user)
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
                    foreach (Claim roleClaim in roleClaims)
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

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }

        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}
