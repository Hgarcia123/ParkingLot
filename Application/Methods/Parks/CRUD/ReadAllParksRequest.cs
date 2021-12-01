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

namespace Application.Methods.Parks.CRUD
{
    public class ReadAllParksRequest : IRequest<List<Park>>
    {
        //public Park Park;

        public ReadAllParksRequest()
        {
            //this.Park = park;
        }
    }

    public class ReadAllParksRequestHandler : MethodsAbstract, IRequestHandler<ReadAllParksRequest, List<Park>>
    {
        public ReadAllParksRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<List<Park>> Handle(ReadAllParksRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();

            var list =  await _context.Parks.Select(p => new Park
            {
                Id = p.Id,
                Name = p.Name,
                Location = p.Location,
                MaxSpots = p.MaxSpots
            }).ToListAsync();

            if(list.Count == 0)
            {
                exception.NoParksCreatedException();
            }

            return list;
        }
    }
}
