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

namespace Application.Methods.Spots.CRUD
{
    public class DeleteSpotRequest : IRequest<string>
    {
        public DeleteSpotRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
    public class DeleteSpotRequestHandler : MethodsAbstract, IRequestHandler<DeleteSpotRequest, string>
    {
        public DeleteSpotRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(DeleteSpotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity =  _context.Spots.Find(request.Id);

                if(entity == null)
                {
                    return "ERROR: SpotId does not exist.";
                }

                _context.Spots.Remove((from s in _context.Spots
                                       where s.Id == entity.Id
                                       select s).Single());

                await _context.SaveChangesAsync(cancellationToken);

                return "Spot Deleted";
            }catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
