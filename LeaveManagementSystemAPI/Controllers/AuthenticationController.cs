using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.Authentication.Login;
using LeaveManagementSystemAPI.Models.Authentication.Signup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;
using System.Security.Cryptography;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaveManagementSystemAPI.MyServices;

namespace LeaveManagementSystemAPI.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]

    public class AuthenticationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMailService _mailService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ILogger<AuthenticationController> logger,
            IMailService mailService,
            IGetApplicationBy getApplicationBy
            )
         {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _mailService = mailService;
            
         }


        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            //check if user is null
            if(registerUser  == null || !ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid object"
                });
            }
            var userExists = await _userManager.FindByEmailAsync(registerUser.Email);
            if (userExists != null)
            {
                return BadRequest(new
                {
                    message = "User already exists"
                });
            }
            var role = registerUser.Role;
            if ( await _roleManager.RoleExistsAsync(role))
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerUser.Email,
                    Email = registerUser.Email,
                    Firstname = registerUser.Firstname,
                    LastName = registerUser.Lastname,
                    Phone = registerUser.Phone,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                
                var result = await _userManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email },Request.Scheme);
                    var userEmail = new List<string> { user.Email! };
                    var message = new MailData(userEmail, "Confirmation email link", "Tinotenda Nyamande", confirmationLink, "tinotendanyamande0784@gmail.com",null,null, null,null); ; ;
                    var res = await _mailService.SendAsync(message, new CancellationToken());
                    if (res == "Sucess")
                    {
                        return Ok("Confirmation email has been sent successfully");
                    }else
                    {
                        return BadRequest(new
                        {
                            message = $"Failed to send confirmation email {res}"
                        });
                    }
                     
                    
                }
                else
                {
                    return BadRequest(new
                    {
                        message = result
                    });
                }
            }else
            {
                return BadRequest($"{role} Role does not exist");
            }  

        }
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(MailData mailData)
        {
            var result = await _mailService.SendAsync(mailData, new CancellationToken());
            if (result == "Success")
            {
                
                return StatusCode(StatusCodes.Status200OK, $"Mail has been sent successfully {result}");
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token , string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK, "Email has been confirmed");
                }else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error");
                }
            }else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login (LoginUser loginUser)
        {
            //check user 
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user  == null)
            {
                return BadRequest(new { message = "Username not found" });
            }
            //check password
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginUser.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new
                {
                    message = "Incorrect password"
                });
            }
            if (user != null && await _userManager.CheckPasswordAsync(user,loginUser.Password))
            {
                //create a claims list
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name , user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim ("username" , user.UserName ,ClaimValueTypes.String)
                };


                //add role to claims list
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim("role", role,ClaimValueTypes.String));
                }
                try
                {
                    //generate access token
                    var jwtToken = GetToken(authClaims);
                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                            //token = jwtToken,
                            expiration = jwtToken.ValidTo
                        }
                        );
                }catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }


            }
            return BadRequest ("Error occured");




        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authsigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authsigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }




    }
}
