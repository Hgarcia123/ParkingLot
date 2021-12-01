using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Application.Methods.Search;
using Application.DtoModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SearchController : ControllerAbstract
    {

        [HttpGet]
        [Route("EntriesBetweenDatesInSpot")]
        //[Authorize(Roles = "AllAccess")]
        public async Task<List<EntryDto>> GetEntriesBetweenDates([FromHeader] int spotId, [FromHeader] DateTimeOffset fromDate, [FromHeader] DateTimeOffset toDate)
        {
            return await Mediator.Send(new ReadEntriesBetwenDatesRequest(spotId, fromDate, toDate));
        }

        [HttpGet]
        [Route("EntriesOfToday")]
        //[Authorize(Roles = "AllAccess")]
        public async Task<List<EntryDto>> GetEntriesToday([FromHeader] int spotId)
        {
            return await Mediator.Send(new ReadEntriesTodayRequest(spotId));
        }

        [HttpGet]
        [Route("FreeSpotsInPark")]
        //[Authorize(Roles = "AllAccess, Guest")]
        public async Task<List<ParkSpotsDto>> GetAvailableSpots([FromHeader] int parkId)
        {
            var listSpots = await Mediator.Send(new ReadFreeSpotsInParkRequest(parkId));
            //await _hubContext.Clients.All.SendAsync("Update Spotlist", listSpots);
            return listSpots;
        }

        [HttpGet]
        [Route("NumberOfOccupiedSpots")]
        //[Authorize(Roles = "AllAccess, Guest")]
        public async Task<IActionResult> GetNumOccupiedSpotsInPark([FromHeader] int parkId)
        {
            return new JsonResult(await Mediator.Send(new ReadNumOfOccupiedSpotsRequest(parkId)));
        }

        [HttpGet]
        [Route("NumberOfFreeSpots")]
        //[Authorize(Roles = "AllAccess, Guest")]
        public async Task<IActionResult> GetNumFreeSpotsInPark([FromHeader] int parkId)
        {
            return new JsonResult(await Mediator.Send(new ReadNumOfFreeSpotsRequest(parkId)));
        }
    }
}
