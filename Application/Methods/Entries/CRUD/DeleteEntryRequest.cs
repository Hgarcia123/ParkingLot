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


namespace Application.Methods.Entries.CRUD
{
    public class DeleteEntryRequest : IRequest<string>
    {
        public DeleteEntryRequest(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }

    public class DeleteEntryRequestHandler : MethodsAbstract, IRequestHandler<DeleteEntryRequest, string>
    {
        public DeleteEntryRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context) { }

        public async Task<string> Handle(DeleteEntryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _context.Entries.Find(request.Id);

                if(entity == null)
                {
                    return "ERROR: Entry Id does not exist";
                }

                _context.Entries.Remove(entity);

                await _context.SaveChangesAsync(cancellationToken);
                return "Entry Deleted";
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }
    }
}
