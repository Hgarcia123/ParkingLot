using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Domain;
using MediatR;

using Application.Methods.Parks.CRUD;
using Application.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ParkingLot_API.Controllers
{

    [Route("api/Park")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParkController : ControllerAbstract
    {
        [HttpGet]
        [Route("GetAllParks")]
        //[Authorize(Roles = "AllAccess, Guest", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Park>> GetAllParks()
        {
            return await Mediator.Send(new ReadAllParksRequest());
        }

        [HttpGet]
        [Route("GetParkId")]
        //[Authorize(Roles = "AllAccess, Guest")]
        public async Task<List<ParkDto>> GetParkId([FromHeader] int parkId)
        {
            return await Mediator.Send(new ReadParkIdRequest(parkId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Park request)
        {
            return new JsonResult(await Mediator.Send(new CreateParkRequest(request)));
        }

        [HttpPut]
        //[Authorize(Roles = "AllAccess")]
        public async Task<IActionResult> Put([FromBody] Park request)
        {
            return new JsonResult(await Mediator.Send(new UpdateParkRequest(request)));
        }

        //[HttpDelete]
        //public async Task<IActionResult> Delete([FromHeader] int Id)
        //{
        //    return new JsonResult(await Mediator.Send(new DeleteParkRequest(Id)));
        //}
    }
}
