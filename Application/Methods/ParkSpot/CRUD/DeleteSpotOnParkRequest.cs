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

namespace Application.Methods.ParkSpot.CRUD
{
    public class DeleteSpotOnParkRequest : IRequest<string>
    {
        public DeleteSpotOnParkRequest(int Id)
        {
            this.Id = Id;
        }

        public int Id;
    }

    public class DeleteSpotOnParkRequestHandler : MethodsAbstract, IRequestHandler<DeleteSpotOnParkRequest, string>
    {
        public DeleteSpotOnParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(DeleteSpotOnParkRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ParkSpots.FindAsync(request.Id);

                if(entity == null)
                {
                    return "Error: ParkSpotId does not exist";
                }
                
                _context.ParkSpots.Remove((from p in _context.ParkSpots
                                           where p.Id == entity.Id
                                           select p).Single());

                await _context.SaveChangesAsync(cancellationToken);

                return "Spot " + entity.SpotId + " on Park " +  entity.ParkId + " removed.";
            }catch(Exception e)
            {
                return e.Message;
            }

        }
    }
}
