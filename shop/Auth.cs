using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace shop
{
    public class Auth
    {
        public static string Issuer => "TM";
        public static string Audience => "APIclients";
        public static int LifetimeInMinutes => 300;

        public static SecurityKey SigningKey =>
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes("VerySecretKeyWithMyNameNastya"));

        public static string GenerateToken(bool isAdmin = false)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, "user"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, isAdmin ? "admin" : "guest")
            };
            ClaimsIdentity identity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                Auth.Issuer,
                Auth.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(Auth.LifetimeInMinutes)),
                signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}