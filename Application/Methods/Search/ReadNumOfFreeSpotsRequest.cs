using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;
using System.Linq;

using Domain;
using Application.Interfaces;
using Application.DtoModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Application.Methods.Search
{
    public class ReadNumOfFreeSpotsRequest : IRequest<string>
    {
        public int ParkId { get; set; }

        public ReadNumOfFreeSpotsRequest(int parkId)
        {
            ParkId = parkId;
        }
    }

    public class ReadNumOfFreeSpotsRequestHandler : MethodsAbstract, IRequestHandler<ReadNumOfFreeSpotsRequest, string>
    {
        public ReadNumOfFreeSpotsRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(ReadNumOfFreeSpotsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var parkEntity = _context.Parks.Find(request.ParkId);

                if (parkEntity == null)
                {
                    return "ERROR: ParkId doest not exist (Id must be greater than 0).";
                }

                int freeSpots = await _context.ParkSpots.Where(ps => ps.Status == false).Where(ps => ps.ParkId == request.ParkId).CountAsync();

                return freeSpots + " out of " + parkEntity.MaxSpots + " spots in Park " + parkEntity.Id + " are free.";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
