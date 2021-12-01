using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;

using MediatR;
using Application.Interfaces;

namespace Application.Methods
{
    public abstract class MethodsAbstract
    {
        protected readonly IMapper _mapper;
        protected readonly IParkingContext _context;

        protected MethodsAbstract(IMapper mapper, IParkingContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
