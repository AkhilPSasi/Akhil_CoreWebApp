using Akhil_CoreWebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Akhil_CoreWebApp.Areas.Identity.Pages.Account.LoginModel;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Akhil_CoreWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly UserManager<Akhil_CoreWebAppUser> _userManager;
        public TokenController(UserManager<Akhil_CoreWebAppUser> userManager)
        {
            this._userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] InputModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my Secret Token Key"));
            //var tokenDescriptor = new SecurityTokenDescriptor()
            //{
            //    Subject = new ClaimsIdentity(authClaims),
            //    Expires = DateTime.Now.AddMinutes(20),
            //    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
            //};
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.CreateToken(tokenDescriptor);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is my Secret Token Key"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7007",
                audience: "https://localhost:7007",
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return Ok(new
            {
                //token = tokenHandler.WriteToken(token),
                //expires = token.ValidTo
                token = tokenString
            });

        }
    }
}
