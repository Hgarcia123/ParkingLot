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
    public class ReadNumOfOccupiedSpotsRequest : IRequest<string>
    {
        public int ParkId { get; set; }

        public ReadNumOfOccupiedSpotsRequest(int parkId)
        {
            ParkId = parkId;
        }
    }

    public class ReadNumOfOccupiedSpotsRequestHandler : MethodsAbstract, IRequestHandler<ReadNumOfOccupiedSpotsRequest, string>
    {
        public ReadNumOfOccupiedSpotsRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(ReadNumOfOccupiedSpotsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var parkEntity = _context.Parks.Find(request.ParkId);

                if(parkEntity == null)
                {
                    return "ERROR: ParkId doest not exist (Id must be greater than 0).";
                }

                int occupiedSpots = await _context.ParkSpots.Where(ps => ps.Status == true).Where(ps => ps.ParkId == request.ParkId).CountAsync();

                return occupiedSpots + " out of " + parkEntity.MaxSpots + " spots in Park " + parkEntity.Id + " are occupied.";

            }catch(Exception e)
            {
                return e.Message;
            }
            
        }
    }


}
