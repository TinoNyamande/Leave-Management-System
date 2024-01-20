using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.Authentication.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeaveManagementSystemAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration , UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _roleManager = roleManager;
            _userManager = userManager;
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            //check user 
            var user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user == null)
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
            if (user != null && await _userManager.CheckPasswordAsync(user, loginUser.Password))
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
                    authClaims.Add(new Claim("role", role, ClaimValueTypes.String));
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
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }


            }
            return BadRequest("Error occured");


        }

    }
}

   
