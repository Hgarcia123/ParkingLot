using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;


namespace Application.Methods.ParkSpot.CRUD
{
    public class UpdateParkSpotRequest : IRequest<string>
    {
        public ParkSpots ParkSpots;

        public UpdateParkSpotRequest(ParkSpots parkSpots)
        {
            ParkSpots = parkSpots;
        }
    }

    public class UpdateParkSpotRequestHandler : MethodsAbstract, IRequestHandler<UpdateParkSpotRequest, string>
    {
        public UpdateParkSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(UpdateParkSpotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ParkSpots.FindAsync(request.ParkSpots.Id);

                if(entity == null)
                {
                    return "ERROR: Id of ParkSpot does not exist (Id's must be greater than 0).";
                }

                //_context.ParkSpots.Update(entity);
                //_context.Spots.Update(entity.Spot);
                //entity.Status = request.ParkSpots.Status;

                await _context.SaveChangesAsync(cancellationToken);

                return "Association of Park " + entity.ParkId + " and Spot " + entity.SpotId + " updated.";
            }catch(Exception e)
            {
                return e.Message;
            }

        }
    }
}
