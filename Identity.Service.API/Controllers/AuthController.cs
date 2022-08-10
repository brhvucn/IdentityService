using Identity.Service.API.Utilities;
using Identity.Service.Core;
using Identity.Service.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Service.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private IConfiguration configuration;
        private ILogger<AuthController> logger;
        public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }
        [HttpPost]
        [Route("generate")]
        public IActionResult GenerateToken(AuthRequest request)
        {
            var validateResult = new AuthRequest.AuthRequestValidator().Validate(request);
            if (validateResult.IsValid)
            {
                //input is valid, now generate the token
                CustomClaimDictionary customClaims = GetCustomClaims();
                TokenGenerator tokenGenerator = new TokenGenerator(this.configuration, this.logger, customClaims);
                var tokenResult = tokenGenerator.GenerateToken(request.UserName);
                return FromResult(tokenResult);
            }
            else
            {
                validateResult.Errors.ForEach(x => this.logger.LogError(x.ErrorMessage));
                return Error(validateResult.Errors);
            }            
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult ValidateToken(string request)
        {
            TokenValidator validator = new TokenValidator(this.configuration);
            var validateResult = validator.ValidateToken(request);
            if (validateResult.Failure)
            {
                this.logger.LogError("Invalid Token: " + request);
                this.logger.LogError(validateResult.Error.Message);
            }
            return FromResult(validateResult);
        }

        private CustomClaimDictionary GetCustomClaims()
        {
            CustomClaimDictionary result = new CustomClaimDictionary();
            result.Add(AuthClaims.Role, "Normal");
            //add any more if needed
            return result;
        }
    }
}
