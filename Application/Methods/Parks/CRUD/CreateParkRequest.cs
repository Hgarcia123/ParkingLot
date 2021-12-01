using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;

namespace Application.Methods.Parks.CRUD
{
    public class CreateParkRequest : IRequest<string>
    {
        public CreateParkRequest(Park park)
        {
            Park = park;

        }
        public Park Park { get; }

    }
    public class CreateParkRequestHandler : MethodsAbstract, IRequestHandler<CreateParkRequest, string>
    {
        
        public CreateParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(CreateParkRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var entity = _context.Parks.Add(request.Park);

                await _context.SaveChangesAsync(cancellationToken);

                return "Park created";
            }catch(Exception e)
            {
                return e.Message;
            }

        }
    }
}
