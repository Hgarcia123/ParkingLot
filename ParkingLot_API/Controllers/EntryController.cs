using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Domain;
using Application.Methods.Entries.CRUD;
using Application.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EntryController : ControllerAbstract
    {

        [HttpGet]
        [Route("EntriesInSpot")]
        public async Task<List<EntryDto>> GetEntriesSpot([FromHeader] int spotId)
        {
            return await Mediator.Send(new ReadEntriesFromSpotRequest(spotId));
        }

        //[HttpGet]
        //[Route("Entries in Park")]
        //public async Task<List<Entry>> GetPark([FromHeader] int spotId)
        //{
        //    return await Mediator.Send(new ReadEntriesFromParkRequest(spotId));
        //}
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Entry request)
        {
            return new JsonResult(await Mediator.Send(new CreateEntryRequest(request)));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromHeader] int id)
        {
            return new JsonResult(await Mediator.Send(new UpdateEntryRequest(id)));
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromHeader] int id)
        //{
        //    return new JsonResult(await Mediator.Send(new DeleteEntryRequest(id)));
        //}
    }
}
