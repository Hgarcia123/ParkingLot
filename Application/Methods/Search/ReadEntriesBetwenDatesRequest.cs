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
    public class ReadEntriesBetwenDatesRequest : IRequest<List<EntryDto>>
    {
        public int SpotId { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }

        public ReadEntriesBetwenDatesRequest(int spotId, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            SpotId = spotId;
            FromDate = fromDate;
            ToDate = toDate;

        }
    }

    public class ReadEntrieBetweenDatesRequestHandler : MethodsAbstract, IRequestHandler<ReadEntriesBetwenDatesRequest, List<EntryDto>>
    {
        public ReadEntrieBetweenDatesRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<EntryDto>> Handle(ReadEntriesBetwenDatesRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();

            if (request.ToDate <= request.FromDate)
            {
                exception.InvalidDatesException();
            }

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Entry, EntryDto>());

            var list = await _context.Entries.Where(e => e.SpotId == request.SpotId)
                .Where(e => e.Arrival >= request.FromDate).Where(e => e.Departure <= request.ToDate).ProjectTo<EntryDto>(configuration).ToListAsync();

            if(list.Count == 0)
            {
                exception.NoEntryException(request.SpotId);
            }

            return list;
        }
    }
}
