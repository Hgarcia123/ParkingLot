using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;
using Application.DtoModels;
using Microsoft.AspNetCore.Identity;

namespace Application.Methods.Authorization
{
    public class LogoutUserRequest : IRequest<string>
    {
        public LogoutUserRequest()
        {
        }
    }

    public class LogoutUserRequestHandler : AuthorizationAbstract, IRequestHandler<LogoutUserRequest, string>
    {
        public LogoutUserRequestHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(userManager, signInManager) { }

        public async Task<string> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _signInManager.SignOutAsync();

                return "User Logout Successfull";
            }
            catch (Exception e)
            {

                return e.Message;
            }    

        }
    }
}
