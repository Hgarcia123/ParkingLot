using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Methods.Parks.CRUD
{
    public class UpdateParkRequest : IRequest<string>
    {
        public UpdateParkRequest(Park park)
        {
            Park = park;
        }

        public Park Park;
    }
    public class UpdateParkRequestHandler : MethodsAbstract, IRequestHandler<UpdateParkRequest, string>
    {
        public UpdateParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(UpdateParkRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Parks.AsNoTracking().Where(p => p.Id == request.Park.Id).SingleOrDefaultAsync();

                if(entity == null)
                {
                    return "ERROR: ParkId doest not exist (Id must be greater than 0).";
                }

                _context.Parks.Update(request.Park);

                await _context.SaveChangesAsync(cancellationToken);

                return "Park Updated";

            }catch(Exception e)
            {
                return e.Message;
            }

        }
    }
}
