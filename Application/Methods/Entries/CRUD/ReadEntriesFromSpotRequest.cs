using Microsoft.EntityFrameworkCore;

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

using AutoMapper.QueryableExtensions;

namespace Application.Methods.Entries.CRUD
{
    public class ReadEntriesFromSpotRequest : IRequest<List<EntryDto>>
    {
        public int spotId;

        public ReadEntriesFromSpotRequest(int id)
        {
            this.spotId = id;
        }
    }

    public class ReadEntriesFromSpotRequestHandler : MethodsAbstract, IRequestHandler<ReadEntriesFromSpotRequest, List<EntryDto>>
    {
        public ReadEntriesFromSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<EntryDto>> Handle(ReadEntriesFromSpotRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Entry, EntryDto>());

            var list = await _context.Entries.Where(e => e.SpotId == request.spotId).ProjectTo<EntryDto>(configuration).ToListAsync();

            if(list.Count == 0)
            {
                exception.NoEntryException(request.spotId);
            }

            return list;
        }
    }
}
