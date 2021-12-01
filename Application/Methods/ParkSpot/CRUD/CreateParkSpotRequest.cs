using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using System.Linq;
using MediatR;

using Domain;
using Application.Interfaces;
using Application.Methods.Spots.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.ParkSpot.CRUD
{
    public class CreateParkSpotRequest : IRequest<string>
    {
        public ParkSpots ParkSpots { get; set; }

        public CreateParkSpotRequest(ParkSpots parkSpots)
        {
            ParkSpots = parkSpots;
        }
    }

    public class CreateParkSpotRequestHandler : MethodsAbstract, IRequestHandler<CreateParkSpotRequest, string>
    {
        public CreateParkSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context){ }
        public async Task<string> Handle(CreateParkSpotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var parkSpotEntity = request.ParkSpots;

                //Check if ParkId exists
                var parkEntity = await _context.Parks.FindAsync(parkSpotEntity.ParkId);

                if(parkEntity == null)
                {
                    return "ERROR: ParkId does not exist (Id's must be greater than 0).";
                }

                var dbParkSpot = await _context.ParkSpots.Include(s => s.Spot).Where(ps => ps.ParkId == request.ParkSpots.ParkId).ToListAsync();
                var numDbParkSpot = dbParkSpot.Count + 1;

                if(numDbParkSpot > parkEntity.MaxSpots)
                {
                    return "ERROR: Maximum number of spots in park " + parkSpotEntity.ParkId + " has been reached.";
                }


                var enumerator = dbParkSpot.GetEnumerator();
                
                while((enumerator.MoveNext()) && (enumerator.Current != null))
                {
                    if (enumerator.Current.Spot.SpotMark == parkSpotEntity.Spot.SpotMark)
                    {
                        return "ERROR: SpotMark already exists in park";
                    }
                }

                //Check if SpotId exists
                //var associationEntity = _context.ParkSpots.Where(ps => ps.ParkId == parkSpotEntity.ParkId && ps.SpotId == parkSpotEntity.SpotId);


                //if(associationEntity != null)
                //{
                //    return "ERROR: Association already exists.";
                //}

                //var newSpot = new Spot() { };
                //_context.Spots.Add(new Spot() { });

                //await _context.SaveChangesAsync(cancellationToken);

                //Override Status of Spot
                parkSpotEntity.Status = false;

                _context.ParkSpots.Add(parkSpotEntity);

                await _context.SaveChangesAsync(cancellationToken);

                return "Association with Park " + parkSpotEntity.ParkId + " and Spot " + parkSpotEntity.SpotId + " established. NUM OF SPOTS CREATED: " + numDbParkSpot;

            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
