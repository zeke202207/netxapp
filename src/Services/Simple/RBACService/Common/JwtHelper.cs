using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service.Common
{
    public class JwtHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="claimKey"></param>
        /// <param name="claimValue"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(string secretKey, string issuer, string audience, IEnumerable<Claim> claims, DateTime expires)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JwtSecurityToken ReadJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ReadJwtToken(token);
        }

    }
}
