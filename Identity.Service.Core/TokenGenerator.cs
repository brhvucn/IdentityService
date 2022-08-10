using Identity.Service.Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Service.Core
{
    public class TokenGenerator
    {
        private IConfiguration configuration;
        private ILogger logger;
        private CustomClaimDictionary customClaims;        
        public TokenGenerator(IConfiguration configuration, ILogger logger, CustomClaimDictionary customClaims)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.customClaims = customClaims;
        }

        public TokenGenerator(IConfiguration configuration, ILogger logger) : this(configuration, logger, new CustomClaimDictionary()){ }

        public Result<string> GenerateToken(string userid)
        {
            try
            {
                string secret = this.configuration["Auth:SecretKey"];
                string issuer = this.configuration["Auth:Issuer"];
                string audience = this.configuration["Auth:Audience"];
                var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
                var tokenHandler = new JwtSecurityTokenHandler();
                List<Claim> claims = GenerateCustomClaims(customClaims); //generate the custom claims from the input
                                                                         //add user claims - mandatory, will be used for finding the user id from the constant "NameIdentifier"
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userid));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Result.Ok<string>(tokenHandler.WriteToken(token));
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                return Result.Fail<string>(new Error("TokenException", ex.Message));
            }
        }

        private List<Claim> GenerateCustomClaims(IDictionary<string, string> claims)
        {
           List<Claim> claimsList = new List<Claim>();
           foreach(var claim in claims)
            {
                claimsList.Add(new Claim(claim.Key, claim.Value));
            }
            return claimsList;
        }
    }
}
