using Identity.Service.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Identity.Service.Core
{
    public class TokenValidator
    {
        private IConfiguration configuration;        
        public TokenValidator(IConfiguration configuration)
        {
            this.configuration = configuration;            
        }
        public Result<bool> ValidateToken(string token)
        {
            try
            {
                string secret = this.configuration["Auth:SecretKey"];
                string issuer = this.configuration["Auth:Issuer"];
                string audience = this.configuration["Auth:Audience"];
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
                return Result.Ok(true);
            }
            catch(Exception ex)
            {                
                return Result.Fail<bool>(new Error("Token Invalid", "The token cannot be validated\n"+ex.Message, 400));
            }            
        }
    }
}
