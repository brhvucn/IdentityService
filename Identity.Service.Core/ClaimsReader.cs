using Identity.Service.Core.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Identity.Service.Core
{
    /// <summary>
    /// Helper class for extracting the claims from the token
    /// Assumes that the token given is already validated.
    /// </summary>
    public class ClaimsReader
    {
        private string token;
        private JwtSecurityTokenHandler handler;
        private JwtSecurityToken securityToken;
        public ClaimsReader(string token)
        {
            this.token = token;
            handler = new JwtSecurityTokenHandler();
            securityToken = handler.ReadToken(token) as JwtSecurityToken;
        }

        public Result<string> ReadClaim(string claimType)
        {
            try
            {
                var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
                return Result.Ok<string>(stringClaimValue);
            } 
            catch(Exception ex)
            {
                return Result.Fail<string>(new Error("Exception reading a claim", ex.Message));
            }
        }
    }
}
