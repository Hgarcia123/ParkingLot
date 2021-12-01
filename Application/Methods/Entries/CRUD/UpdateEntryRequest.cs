using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;



namespace Application.Methods.Entries.CRUD
{
    public class UpdateEntryRequest : IRequest<string>
    {
        public UpdateEntryRequest(int spotId)
        {
            SpotId = spotId;
        }

        public int SpotId { get; }
    }
    public class UpdateEntryRequestHandler : MethodsAbstract, IRequestHandler<UpdateEntryRequest, string>
    {
        public UpdateEntryRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(UpdateEntryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _context.Entries.Where(e => e.SpotId == request.SpotId).OrderBy(e => e.Id).LastOrDefault();

                if(entity == null)
                {
                    return "ERROR: No entries for Spot " + request.SpotId + " found.";
                }

                var parkSpotEntity = _context.ParkSpots.Find(entity.SpotId);

                if(parkSpotEntity.Status == false)
                {
                    return "ERROR: Cannot close entry of non occupied spot!";
                }

                entity.Departure = DateTimeOffset.Now;


                //Calculate Payment
                var timeElapsed = (entity.Departure - entity.Arrival).Value.TotalHours;
                entity.TotalPay = (float)Math.Round((float)timeElapsed * entity.PricePerHour, 2);
                
                //Change status of spot to free
                parkSpotEntity.Status = false;

                await _context.SaveChangesAsync(cancellationToken);

                return "Entry Closed on date " + entity.Departure + " with total pay = " + entity.TotalPay + "€";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
