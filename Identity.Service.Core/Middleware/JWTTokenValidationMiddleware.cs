using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Service.Core.Middleware
{
    public class JWTTokenValidationMiddleware
    {
        private readonly RequestDelegate nextRequest;
        private readonly IConfiguration configuration;
        public JWTTokenValidationMiddleware(RequestDelegate nextRequest, IConfiguration config)
        {
            this.nextRequest = nextRequest;
            this.configuration = config;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Handling request in middleware: " + context.TraceIdentifier);
            var token = context.Request.Headers["X-Auth-Token"];
            //check if the token exist, if not, reject the request
            if (!string.IsNullOrEmpty(token))
            {
                //we have the token, now validate it
                TokenValidator validator = new TokenValidator(configuration);
                bool isTokenValid = validator.ValidateToken(token);
                if (isTokenValid)
                {
                    //the token is present and valid, allow the request to proceed
                    await nextRequest(context);
                }
                else
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("UnAuthorized. The provided JWT Token can not be validated");
                    return;
                }
            }
            else
            {
                //we do not have the token, create a response and send it without calling "nextRequest"
                context.Response.StatusCode = 400; //Bad request
                await context.Response.WriteAsync("Bad Request. No JWT token was found in the header");
                return;
            }
            
        }
    }
}
