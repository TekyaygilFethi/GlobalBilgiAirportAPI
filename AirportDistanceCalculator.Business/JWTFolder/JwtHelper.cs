using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AirportDistanceCalculator.Business.JWTFolder
{
    public static class JwtHelper
    {
        public static string GetJwtToken(string userId, IConfiguration configuration, TimeSpan expiration, Claim[] additionalClaims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if(additionalClaims != null && additionalClaims.Any())
            {
                var claimList = new List<Claim>(claims);
                claimList.AddRange(additionalClaims);
                claims = claimList.ToArray();
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings:SigningKey")?.Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration.GetSection("JwtSettings:Issuer")?.Value,
                audience: configuration.GetSection("JwtSettings:Audience")?.Value,
                claims: claims,
                expires: DateTime.Now.Add(expiration),
                signingCredentials: creds
            );

            return tokenHandler.WriteToken(token);


        }
    }
}
