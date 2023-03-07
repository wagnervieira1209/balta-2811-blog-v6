using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BlogV6.Extensions;
using BlogV6.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BlogV6.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey); // Chave em bytes

            var claims = user.GetClaims();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                /*Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name), // User.Identity.Name
                    new Claim(ClaimTypes.Role, user.Roles.First().Name)  // User.Identity.Role
                }),*/
                Expires = DateTime.UtcNow.AddHours(2), // ExpirationTIme
                // Algorítimo de autenticação
                SigningCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}