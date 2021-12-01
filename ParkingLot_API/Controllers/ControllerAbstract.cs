using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Linq;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ControllerAbstract : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        //public void OnActionExecuting(ActionExecutedContext context)
        //{
        //    if (!context.ModelState.IsValid)
        //    {
        //        IDictionary<string, IEnumerable<string>> errors =
        //            new Dictionary<string, IEnumerable<string>>();

        //        foreach(KeyValuePair<string, ModelState> state in context.ModelState)
        //        {
        //            errors[state.Key] = state.Value.Errors.Select(e => e.ErrorMessage);
        //        }

        //        context.Response = context.Request.CreateResponse(HttpStatusCode.Unauthorized, errors);
        //    }
        //}
    }
}
