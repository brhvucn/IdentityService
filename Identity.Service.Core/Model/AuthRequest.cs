using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Service.Core.Model
{
    /// <summary>
    /// Auth request that is received in the api controller layer
    /// </summary>
    public class AuthRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public class AuthRequestValidator : AbstractValidator<AuthRequest>
        {
            public AuthRequestValidator()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }            
        }
    }
}
