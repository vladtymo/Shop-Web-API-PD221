using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    internal class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly JwtOptions jwtOpts;

        public JwtService(IConfiguration configuration, UserManager<User> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            // TODO: use DI
            this.jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.DateOfBirth, user.Birthdate.ToString()),
                new Claim("ClientType", user.ClientType.ToString()),
            };

            var roles = userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            return claims;
        }

        public string CreateToken(IEnumerable<Claim> claims)
        {
            // TODO: make separate method
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpts.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtOpts.AccessTokenLifetimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public IEnumerable<Claim> GetClaimsFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOpts.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpts.Key)),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken;

            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg
                    .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new HttpException(Errors.InvalidToken, HttpStatusCode.BadRequest);
            }

            return jwtSecurityToken.Claims;
        }
    }
}
