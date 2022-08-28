using LAFDalApp.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Policy;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAFWebApp
{
    [Route("Auth/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
            //var user1 = await userManager.FindByIdAsync(loginVM.Id);
            User user = UserBAL.Instance.getUser(0);
            //var user = await userManager.FindByNameAsync(loginVM.UserName);
            if (user != null)// && await userManager.CheckPasswordAsync(user, loginVM.Password))
            {
                //var userRoles = await userManager.GetRolesAsync(user);

                List<string> permissions = UserBAL.Instance.getPermissions();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.MobilePhone,"asd"),
                    new Claim(ClaimTypes.Email,"asd@asd.com"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var permission in permissions)
                {
                    authClaims.Add(new Claim("Permission", permission));
                    authClaims.Add(new Claim(ClaimTypes.Role, permission));
                }

                

                //var token = GetToken(authClaims);

                //return Ok(new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken(token),
                //    expiration = token.ValidTo
                //});


                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456"));//_configuration["JsonWebTokenKeys:IssuerSigningKey"]));

                var token = new JwtSecurityToken(
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                //this.signInManager.SignInAsync(user, true);
                return Ok(new
                {
                    api_key = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    user = user,
                    //Role = userRoles,
                    permissions = permissions,
                    status = "User Login Successfully"
                });
            }
            return Unauthorized();
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("64A63153-11C1-4919-9133-EFAF99A9B456"));

            var token = new JwtSecurityToken(
                //ValidateIssuer: false,
                //ValidateAudience: false,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
