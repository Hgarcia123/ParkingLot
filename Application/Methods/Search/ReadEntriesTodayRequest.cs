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

using Application.DtoModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Application.Methods.Search
{
    public class ReadEntriesTodayRequest : IRequest<List<EntryDto>>
    {
        public int SpotId { get; set; }

        public ReadEntriesTodayRequest(int spotId)
        {
            SpotId = spotId;
        }
    }

    public class ReadEntriesTodayRequestHandler : MethodsAbstract, IRequestHandler<ReadEntriesTodayRequest, List<EntryDto>>
    {
        public ReadEntriesTodayRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<EntryDto>> Handle(ReadEntriesTodayRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Entry, EntryDto>());


            var list = await _context.Entries.Where(e => e.SpotId == request.SpotId)
                .Where(e => e.Arrival.Value.DayOfYear == DateTimeOffset.Now.DayOfYear)
                .Where(e => e.Arrival.Value.Year == DateTimeOffset.Now.Year)
                .ProjectTo<EntryDto>(configuration).ToListAsync();

            if(list.Count == 0)
            {
                exception.NoEntryException(request.SpotId);
            }

            return list;
        }
    }
}
