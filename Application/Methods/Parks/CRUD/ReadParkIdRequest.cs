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
using Application.DtoModels;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Application.Methods.Parks.CRUD
{
    public class ReadParkIdRequest : IRequest<List<ParkDto>>
    {
        public ReadParkIdRequest(int id)
        {
            Id = id;
        }

        public int Id{ get; }
    }

    public class ReadParkIdRequestHandler : MethodsAbstract, IRequestHandler<ReadParkIdRequest, List<ParkDto>>
    {
        public ReadParkIdRequestHandler(IMapper mapper, IParkingContext context) : base(mapper, context){ }
        public async Task<List<ParkDto>> Handle(ReadParkIdRequest request, CancellationToken cancellationToken)
        {
            ArgException exception = new ArgException();

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<Park, ParkDto>());

            var entity = await _context.Parks.Where(p => p.Id == request.Id).ProjectTo<ParkDto>(configuration).ToListAsync();

            if(entity.Count() == 0)
            {
                exception.NoParkException(request.Id);
            }

            return entity;
        }
    }
}
