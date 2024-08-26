using AgentsRest.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AgentsRest.Dto;
namespace AgentsRest.Service
{
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        public string CreateToken(string id )
        {
            string key = configuration.GetValue("Jwt:Key", string.Empty)
                ?? throw new ArgumentNullException("Key doesnt exists on Jwt");

            int expiry = configuration.GetValue("Jwt:ExpiryInMinutes", 60);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentils = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims =
            {
                new (ClaimTypes.NameIdentifier, id)
            };

            var token = new JwtSecurityToken(
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Audience"],
               expires: DateTime.Now.AddMinutes(expiry),
               claims: claims,
               signingCredentials: credentils
           );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}