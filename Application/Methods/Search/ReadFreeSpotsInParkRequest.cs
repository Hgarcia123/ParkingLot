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
    public class ReadFreeSpotsInParkRequest : IRequest<List<ParkSpotsDto>>
    {
        public int ParkId;

        public ReadFreeSpotsInParkRequest(int parkId)
        {
            ParkId = parkId;
        }
    }

    public class ReadFreeSpotsInParkRequestHandler : MethodsAbstract, IRequestHandler<ReadFreeSpotsInParkRequest, List<ParkSpotsDto>>
    {
        public ReadFreeSpotsInParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context){ }

        public async Task<List<ParkSpotsDto>> Handle(ReadFreeSpotsInParkRequest request, CancellationToken cancellationToken)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<ParkSpots, ParkSpotsDto>());
            return await _context.ParkSpots.Where(ps => ps.Status == false).Where(ps => ps.ParkId == request.ParkId).ProjectTo<ParkSpotsDto>(configuration).ToListAsync();
        }
    }
}
