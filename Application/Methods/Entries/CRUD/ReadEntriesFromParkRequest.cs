using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


using MediatR;
using AutoMapper;

using Domain;
using Application.Interfaces;


namespace Application.Methods.Entries.CRUD
{
    public class ReadEntriesFromParkRequest : IRequest<List<Entry>>
    {
        public int id;

        public ReadEntriesFromParkRequest(int id)
        {
            this.id = id;
        }
    }

    public class ReadEntriesFromParkRequestHandler : MethodsAbstract, IRequestHandler<ReadEntriesFromParkRequest, List<Entry>>
    {
        public ReadEntriesFromParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<Entry>> Handle(ReadEntriesFromParkRequest request, CancellationToken cancellationToken)
        {

            return await _context.Entries.Include(ps => ps.Spots.ListParks).Where(e => e.SpotId == request.id).ToListAsync();
        }
    }
}
