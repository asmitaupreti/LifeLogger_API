
using System.Net;
using LifeLogger.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LifeLogger.API.Middleware.Exceptions
{
    public static class CustomBadRequestException 
    {
        
        public static IActionResult ConstructErrorMessages(ActionContext context)
        {
            APIResponse _response = new();
            _response.IsSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            foreach (var error in context.ModelState)
            {
                var key = error.Key;
                var errors = error.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    _response.ErrorMessage = errors.Select(e => e.ErrorMessage).ToList();
                }
            }
            return new BadRequestObjectResult(_response);
        }
    }
}