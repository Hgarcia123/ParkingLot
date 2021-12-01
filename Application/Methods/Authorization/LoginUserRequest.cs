using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using MediatR;

using Domain;
using Application.Interfaces;
using Application.DtoModels;
using Microsoft.AspNetCore.Identity;

namespace Application.Methods.Authorization
{
    public class LoginUserRequest : IRequest<string>
    {
        public LoginUserDto LoginUserDto;

        public LoginUserRequest(LoginUserDto loginUserDto)
        {
            this.LoginUserDto = loginUserDto;
        }
    }

    public class LoginUserRequestHandler : AuthorizationAbstract, IRequestHandler<LoginUserRequest, string>
    {
        public LoginUserRequestHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(userManager, signInManager) { }

        public async Task<string> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var login = await _signInManager.PasswordSignInAsync(request.LoginUserDto.UserName, request.LoginUserDto.Password, true, true);

                if (login.Succeeded)
                {
                    return "User login successful";
                }
                return "Username or Password incorrect!";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
