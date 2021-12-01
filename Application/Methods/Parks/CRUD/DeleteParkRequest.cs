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

namespace Application.Methods.Parks.CRUD
{
    public class DeleteParkRequest : IRequest<string>
    {
        public DeleteParkRequest(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }

    public class DeleteParkRequestHandler : MethodsAbstract, IRequestHandler<DeleteParkRequest, string>
    {
        public DeleteParkRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(DeleteParkRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _context.Parks.Find(request.Id);

                if(entity == null)
                {
                    return "ERROR: Park Id does not exist";
                }
                _context.Parks.Remove(entity);

                //await _mediator.Publish()
                await _context.SaveChangesAsync(cancellationToken);

                return "Park" + request.Id +  "Deleted";
            }catch(Exception e)
            {
                return e.Message;
            }
            
        }
    }
}
