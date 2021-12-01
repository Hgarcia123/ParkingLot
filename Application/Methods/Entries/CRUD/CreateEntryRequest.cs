using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;


namespace Application.Methods.Entries.CRUD
{
    public class CreateEntryRequest : IRequest<string>
    {
        public CreateEntryRequest(Entry Entry/*int parkSpotsParkId, int parkSpotsSpotId, string plate, int pricePerHour*/)
        {
            this.Entry = Entry;
            //    ParkSpotsParkId = parkSpotsParkId;
            //    ParkSpotsSpotId = parkSpotsSpotId;
            //    Plate = plate;
            //    PricePerHour = pricePerHour;
        }

        public Entry Entry;
    //public int ParkSpotsParkId { get; }
    //public int ParkSpotsSpotId { get; }
    //public string Plate { get; }
    //public int PricePerHour { get; }
}

    public class CreateEntryRequestHandler : MethodsAbstract, IRequestHandler<CreateEntryRequest, string>
    {
        public CreateEntryRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(CreateEntryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = request.Entry;

                var parkSpotsEntity = await _context.ParkSpots.FindAsync(entity.SpotId);

                if(parkSpotsEntity == null)
                {
                    return "ERROR: ParkId and/or SpotId do not exist (Id's must be greater than 0).";
                }

                if(parkSpotsEntity.Status == true)
                {
                    return "ERROR: Cannot create entry because spot is occupied.";
                }

                //Change status of spot to occupied
                parkSpotsEntity.Status = true;

                _context.Entries.Add(entity);

                entity.Arrival = DateTimeOffset.Now;
                entity.Departure = DateTimeOffset.MinValue;

                await _context.SaveChangesAsync(cancellationToken);

                return "Entry Created";
            }catch(Exception e)
            {
                return e.Message;
            }

        }
    }
    
}
