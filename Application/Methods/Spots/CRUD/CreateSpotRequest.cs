using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;


namespace Application.Methods.Spots.CRUD
{
    public class CreateSpotRequest : IRequest<string>
    {
        public Spot Spot;

        public CreateSpotRequest(Spot spot)
        {
            Spot = spot;
        }
    }

    public class CreateSpotRequestHandler : MethodsAbstract, IRequestHandler<CreateSpotRequest, string>
    {
        public CreateSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(CreateSpotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Spots.Add(request.Spot);
                await _context.SaveChangesAsync(cancellationToken);

                return "Spot Created";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
