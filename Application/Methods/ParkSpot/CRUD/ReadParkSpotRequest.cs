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

namespace Application.Methods.ParkSpot.CRUD
{
    public class ReadParkSpotRequest : IRequest<List<ParkSpotsDto>>
    {
        public ReadParkSpotRequest(int parkId)
        {
            ParkId = parkId;
        }

        public int ParkId { get; }
    }

    public class ReadParkSpotRequestHandler : MethodsAbstract, IRequestHandler<ReadParkSpotRequest, List<ParkSpotsDto>>
    {
        public ReadParkSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }
        public async Task<List<ParkSpotsDto>> Handle(ReadParkSpotRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ParkSpots, ParkSpotsDto>());

            var list = await _context.ParkSpots.Include(s => s.Spot).Where(ps => ps.ParkId == request.ParkId).ProjectTo<ParkSpotsDto>(configuration).ToListAsync();

            if(list.Count == 0)
            {
                exception.NoParkSpotException(request.ParkId);
            }

            return list;
        }
    }
}
