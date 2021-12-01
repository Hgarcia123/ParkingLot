using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.Spots.CRUD
{
    public class UpdateSpotRequest : IRequest<string>
    {
        public Spot Spot;

        public UpdateSpotRequest(Spot spot)
        {
            Spot = spot;
        }
    }

    public class UpdateSpotRequestHandler : MethodsAbstract, IRequestHandler<UpdateSpotRequest, string>
    {
        public UpdateSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(UpdateSpotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Spots.AsNoTracking().Where(p => p.Id == request.Spot.Id).SingleOrDefaultAsync();

                if (entity.Id == 0)
                {
                    return "ERROR: SpotId does not exist.";
                }

                var park = await _context.ParkSpots.AsNoTracking().Where(p => p.SpotId == entity.Id).FirstOrDefaultAsync();

                var dbParkSpot = await _context.ParkSpots.AsNoTracking().Include(s => s.Spot).Where(ps => ps.ParkId == park.ParkId).ToListAsync();
                //var enumerator = dbParkSpot

                var enumerator = dbParkSpot.GetEnumerator();

                while ((enumerator.MoveNext()) && (enumerator.Current != null))
                {
                    if (enumerator.Current.Spot.SpotMark == request.Spot.SpotMark)
                    {
                        return "ERROR: SpotMark already exists in park";
                    }
                }

                _context.Spots.Update(request.Spot);

                await _context.SaveChangesAsync(cancellationToken);

                return "Spot Updated";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
