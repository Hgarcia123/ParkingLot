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
using Microsoft.EntityFrameworkCore;

namespace Application.Methods.Spots.CRUD
{ 
    public class ReadSpotRequest : IRequest<List<Spot>>
    {
        public int SpotId { get; }

        public ReadSpotRequest(int spotId)
        {
            SpotId = spotId;
        }
    }

    public class ReadSpotRequestHandler : MethodsAbstract, IRequestHandler<ReadSpotRequest, List<Spot>>
    {
        public ReadSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<Spot>> Handle(ReadSpotRequest request, CancellationToken cancellationToken)
        {
            return await _context.Spots.Where(s => s.Id == request.SpotId).ToListAsync(cancellationToken);
        }
    }
}
