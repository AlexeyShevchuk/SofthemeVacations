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
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Vacations.BLL.Services;
using Vacations.DAL.Models;

namespace Vacations.API.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUsersService _usersService;
        //private IEmailSender _emailSender;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            AccountsDbContext context,
            RoleManager<IdentityRole> roleManager,
            //IEmailSender emailSender
            IUsersService usersService
        )
        {
            _usersService = usersService;
            //_emailSender = emailSender;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("token")]
        public async Task<IActionResult> GetTokenAsync()
        {
            try
            {
                var header = Request.Headers["Authorization"];

                return Ok(await _usersService.GetTokenAsync(header.ToString()));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public class ForgotPasswordViewModel
        {
            public string Email { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel email)
        {
            if (ModelState.IsValid)
            {
                await _usersService.ForgotPassword(email.Email);
                return Ok();
            }

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