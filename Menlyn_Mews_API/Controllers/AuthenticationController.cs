using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Menlyn_Mews_API.Models;
using Menlyn_Mews_API.Models.Domain.Login;
using Menlyn_Mews_API.Models.Domain.SignUp;
using Menlyn_Mews.Service.Models;
using Menlyn_Mews.Service.Services;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IRepositroy _repository;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            SignInManager<ApplicationUser> signInManager,
            IRepositroy repositroy)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _signInManager = signInManager;
            _repository = repositroy;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, string role)
        {
            //Exist?
            var userExist = await _userManager.FindByEmailAsync(registerUser.Email);

            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new Response { Status = "Error", Message = "User Exists!" });
            }

            //Add To Db
            var user = new ApplicationUser
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.UserName,
                TwoFactorEnabled = true,
            };

            if (await _roleManager.RoleExistsAsync(role))
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Failed To Create!" });
                }
                //Add Role To User

                await _userManager.AddToRoleAsync(user, role);

                //Client
                var client = new Client
                {
                    Client_Name = registerUser.Client_Name,
                    Client_Surname = registerUser.Client_Surname,
                    Client_ID_Number = registerUser.Client_ID_Number,
                    Client_Email_Address = registerUser.Client_Email_Address,
                    Client_Contact_Number = registerUser.Client_Contact_Number,
                    Client_Gender = registerUser.Client_Gender,
                    Title = registerUser.Title,
                    ApplicationUserId = user.Id,
                };

                _repository.Add(client);
                await _repository.SaveChangesAsync();

                //Add Token To Verify Email

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Confirmation Email Link", confirmationLink!);
                _emailService.SendEmail(message);



                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"User Created & Email Sent to {user.Email} Succcessfully" });

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Doesnt Exist!" });
            }

        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "Email Verified Successfully" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new Response { Status = "Error", Message = "Doesnt Exist" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {

            //Checking User
            var user = await _userManager.FindByNameAsync(loginModel.Username);

            if (user == null)
            {
                return Unauthorized(new Response { Status = "Error", Message = "Invalid username or password." });
            }

            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return Unauthorized(new Response { Status = "Error", Message = "Invalid username or password." });
            }

            if (user.TwoFactorEnabled)
            {
                //Send OTP
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, true);
                var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"We have sent an OTP to your email {user.Email}" });
            }

            //Claimlist Creation
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //Add Roles to Claimlist
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            //Generate Token
            var jwtToken = GetToken(authClaims);

            //Return Token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo
            });

        }

        [HttpPost]
        [Route("Login-2FA")]
        public async Task<IActionResult> LoginWithOTP(string code, string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User not found." });
            }

            // Check if the OTP code is correct
            var isTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, "Email", code);

            if (!isTokenValid)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "Invalid Code" });
            }

            // Claimlist Creation
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add Roles to Claimlist
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate Token
            var jwtToken = GetToken(authClaims);

            var clientInfo = await _repository.GetClientByAppUserIdAsync(user.Id);

            // Return Token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo,
                roles = userRoles.FirstOrDefault(),
                clientId = clientInfo.ClientId,
            });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new Response { Status = "Success", Message = "User logged out successfully." });
        }

        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordlink = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme);
                var message = new Message(new string[] { user.Email! }, "Forgot Password Email Link", forgotPasswordlink!);
                _emailService.SendEmail(message);


                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"We have sent Password Reset Link to your email {user.Email}. Please Copy The OTP/Token For Successful Password Reset" });

            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = "Error", Message = "Could Not Send Link To Email!" });
        }

        [HttpGet]
        [Route("IsLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated;
            return Ok(new { isLoggedIn });
        }


        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };

            return Ok(new
            {
                model
            });
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user != null)
            {
                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }

                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"Password Reset Successfully" });

            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = "Error", Message = "Could Not Send Link To Email!" });
        }

        //Token Creator
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
