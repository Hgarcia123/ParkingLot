using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Authorization;

using Domain;
using Application.Methods.Spots.CRUD;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SpotController : ControllerAbstract
    {

        [HttpGet]
        //[Authorize(Roles = "AllAccess, Guest, ")]
        public async Task<List<Spot>> Get([FromHeader] int id)
        {
            return await Mediator.Send(new ReadSpotRequest(id));
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Spot request)
        //{
        //    return new JsonResult(await Mediator.Send(new CreateSpotRequest(request)));
        //}

        [HttpPut]
        //[Authorize(Roles = "AllAccess")]
        public async Task<IActionResult> Put([FromBody] Spot request)
        {
            return new JsonResult(await Mediator.Send(new UpdateSpotRequest(request)));
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromHeader] int id)
        //{
        //    return new JsonResult(await Mediator.Send(new DeleteSpotRequest(id)));
        //}
    }
}
