using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain;
using Microsoft.AspNetCore.Authorization;

using Application.Methods.ParkSpot.CRUD;
using Application.DtoModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParkSpotController : ControllerAbstract
    {
        [HttpGet]
        [Route("SpotsInPark")]
        //[Authorize(Roles = "AllAccess, Guest")]
        public async Task<List<ParkSpotsDto>> Get([FromHeader] int parkId)
        {
            return await Mediator.Send(new ReadParkSpotRequest(parkId));
        }
        [HttpPost]
        //[Authorize(Roles = "AllAccess")]
        public async Task<IActionResult> Post([FromBody] ParkSpots request)
        {
            return new JsonResult(await Mediator.Send(new CreateParkSpotRequest(request))); 
        }

        //[HttpPut]
        //public async Task<IActionResult> Put([FromBody] ParkSpots request)
        //{
        //    return new JsonResult(await Mediator.Send(new UpdateParkSpotRequest(request)));
        //}

        [HttpDelete]
        //[Authorize(Roles = "AllAccess")]
        public async Task<IActionResult> Delete([FromHeader] int Id)
        {
            return new JsonResult(await Mediator.Send(new DeleteSpotOnParkRequest(Id)));
        }

    }
}
