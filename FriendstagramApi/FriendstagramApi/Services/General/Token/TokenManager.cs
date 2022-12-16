using FriendstagramApi.Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FriendstagramApi.Services.General.Token
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;
        public TokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateAccessToken(User user)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("id", user.UserId.ToString()),
                new Claim("email", user.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
                (
                    claims: claimsIdentity.Claims,
                    signingCredentials: signingCredentials,

                    expires: DateTime.Now.AddMinutes(500),
                    notBefore: DateTime.Now
                );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string tokenString = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return tokenString;
        }


    }
}
