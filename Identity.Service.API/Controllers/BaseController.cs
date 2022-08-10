using FluentValidation.Results;
using Identity.Service.API.Utilities;
using Identity.Service.Core.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Service.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected new ActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected ActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected ActionResult Error(List<string> errorMessages)
        {
            string errors = string.Join(";", errorMessages);
            return BadRequest(Envelope.Error(errors));
        }

        protected ActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected ActionResult Error(List<ValidationFailure> validationErrors)
        {
            List<string> errors = validationErrors.Select(x => x.ErrorMessage + "(" + x.PropertyName + ")").ToList();
            return Error(errors);
        }

        protected ActionResult Error(Error error)
        {
            return BadRequest(Envelope.Error(error.Message + " (" + error.Code + ")"));
        }

        protected IActionResult FromResult(Result result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);

            return base.Ok(Envelope.Ok(result.Value));
        }

        private IActionResult StatusCodeFromResult(Result result)
           => StatusCode(result.Error.StatusCode, Envelope.Error(result.Error.Code));
    }
}
