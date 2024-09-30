using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Menlyn_Mews_API.Models.Domain.Login;
using Menlyn_Mews_API.Models.Domain.SignUp;
using Menlyn_Mews.Service.Models;
using Menlyn_Mews.Service.Services;
using Menlyn_Mews_API.Models.Domain;
using Menlyn_Mews_API.Models.Repositories;
using Menlyn_Mews_API.Data;
using Menlyn_Mews_API.Models.Domain.Emails;
using Microsoft.EntityFrameworkCore;

namespace Menlyn_Mews_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IRepositroy _repository;
        private readonly IGeneralEmailService _generateEmailService;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailService emailService,
            SignInManager<ApplicationUser> signInManager,
            IRepositroy repositroy,
            AppDbContext context, IGeneralEmailService generalEmailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
            _signInManager = signInManager;
            _repository = repositroy;
            _context = context;
            _generateEmailService = generalEmailService;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                    Client_Email_Address = registerUser.Email,
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

        [HttpPost("RegisterEmployee/{role}")]
        public async Task<IActionResult> RegisterEmployee([FromBody] RegisterEmployee registerUser, string role)
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
                var employee = new Employee
                {
                    Employee_Name = registerUser.Employee_Name,
                    Employee_Surname = registerUser.Employee_Surname,
                    Employee_ID_Number = registerUser.Employee_ID_Number,
                    Employee_Email_Address = registerUser.Employee_Email_Address,
                    Employee_Contact_Number = registerUser.Employee_Contact_Number,
                    Employee_Gender = registerUser.Employee_Gender,
                    Employee_Address = registerUser.Employee_Address,
                    EmployeeTypeId = registerUser.EmployeeTypeId,
                    PositionId = registerUser.PositionId,
                    RateId = registerUser.RateId,
                    Employee_Photo = registerUser.Employee_Photo,
                    ApplicationUserId = user.Id,
                };

                _repository.Add(employee);
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

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            List<IdentityRole> roles = new List<IdentityRole>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                roles.Add(role);
            }

            return Ok(roles);
        }

        [HttpPost]
        [Route("RemoveRoles")]
        public async Task<IActionResult> RemoveRole(string userName, [FromBody] string[] roleToRemove)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound();
            }


            var result = await _userManager.RemoveFromRolesAsync(user, roleToRemove);
            if (result.Succeeded)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var role in roleToRemove)
                {
                    sb.Append(role.ToString());
                }
                return Ok("Roles " + sb.ToString() + " Successfully Removed");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("AssignRole")]
        public async Task<IActionResult> AssignRole(string userName, string role)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound(); //User Exists?
            }


            if (!await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Role does not exist."); //Role Provided Exists
            }


            if (await _userManager.IsInRoleAsync(user, role))
            {
                return Ok($"User already has the '{role}' role."); //Check If The Provided Role Is Already Assigned
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to remove current role(s).");
                }
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                return Ok("Role assigned successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to assign role.");
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

                //var message = new Message(new string[] { user.Email! }, "OTP Confirmation", token);
                //_emailService.SendEmail(message);

                var mailrequest = new Mailrequest
                {
                    ToEmail = user.Email,
                    Subject = "Menlyn Mews Login OTP",
                    Body = GenerateOTPEmailBody(token)
                };

                await _generateEmailService.SendEmailAsync(mailrequest);

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
            var employeeInfo = await _repository.GetEmployeeByAppUserIdAsync(user.Id);
            // Return Token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = jwtToken.ValidTo,
                roles = userRoles?.FirstOrDefault(),
                clientId = clientInfo?.ClientId,
                employeeId = employeeInfo?.EmployeeId
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
                var userEmail = Uri.EscapeDataString(user.Email);
                var encodedToken = Uri.EscapeDataString(token);
                var forgotPasswordlink = $"http://localhost:4200/reset/{userEmail}/{encodedToken}";


                var mailrequest = new Mailrequest
                {
                    ToEmail = email,
                    Subject = "Reset Password",
                    Body = GenerateForgotEmailBody(forgotPasswordlink),
                };

                await _generateEmailService.SendEmailAsync(mailrequest);

                return StatusCode(StatusCodes.Status200OK,
                    new Response { Status = "Success", Message = $"We have sent Password Reset Link to your email {user.Email}. Please check your inbox." });
            }

            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = "Error", Message = "Could Not Send Link To Email!" });
        }

        private string GenerateForgotEmailBody( string link)
        {
            var htmlContent = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            width: 100%;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                            max-width: 600px;
                            margin: 50px auto;
                        }}
                        .header {{
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 20px;
                            text-align: center;
                            border-top-left-radius: 10px;
                            border-top-right-radius: 10px;
                        }}
                        .content {{
                            padding: 20px;
                            font-size: 16px;
                            line-height: 1.6;
                            text-align: center;
                        }}
                        .content p {{
                            margin-bottom: 20px;
                        }}
                        .button {{
                            display: inline-block;
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        .footer {{
                            padding: 20px;
                            text-align: center;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <p>Dear User,</p>
                            <p>We received a request to reset your password. If you made this request, please click the button below to reset your password.</p>
                            <p><a href='{link}' class='button'>Reset Password</a></p>
                            <p>If you did not make this request, please ignore this email.</p>
                        </div>
                        <div class='footer'>
                            <p>Thank you, <br/>Menlyn Mews Team</p>
                        </div>
                    </div>
                </body>
                </html>";

            return htmlContent;

        }

        private string GenerateOTPEmailBody(string OTP)
        {
            var htmlContent = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            width: 100%;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 10px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                            max-width: 600px;
                            margin: 50px auto;
                        }}
                        .header {{
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 20px;
                            text-align: center;
                            border-top-left-radius: 10px;
                            border-top-right-radius: 10px;
                        }}
                        .content {{
                            padding: 20px;
                            font-size: 16px;
                            line-height: 1.6;
                            text-align: center;
                        }}
                        .content p {{
                            margin-bottom: 20px;
                        }}
                        .button {{
                            display: inline-block;
                            background-color: #007bff;
                            color: #ffffff;
                            padding: 10px 20px;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        .footer {{
                            padding: 20px;
                            text-align: center;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>OTP</h1>
                        </div>
                        <div class='content'>
                            <p>Dear User</p>
                            <p>Your One-Time-Pin Code Is Below. Please Do Not Share This</p>
                            <p><a class='button'>{OTP}</a></p>
                            <p>If you did not make this request, please ignore this email.</p>
                        </div>
                        <div class='footer'>
                            <p>Thank you, <br/>Menlyn Mews Team</p>
                        </div>
                    </div>
                </body>
                </html>";

            return htmlContent;

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
