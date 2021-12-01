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
    public class RegisterUserRequest : IRequest<string>
    {
        //public string UserName { get; set; }
        //public string Password { get; set; }
        //public string Email { get; set; }

        public CreateUserDto UserDto;

        public RegisterUserRequest(CreateUserDto userDto/*string userName, string password, string email*/)
        {
            UserDto = userDto;
            //UserName = userName;
            //Password = password;
            //Email = email;
        }
    }

    public class RegisterUserRequestHandler : AuthorizationAbstract, IRequestHandler<RegisterUserRequest, string>
    {
        public RegisterUserRequestHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(userManager, signInManager) { }

        public async Task<string> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = request.UserDto.UserName,
                    Email = request.UserDto.Email
                };

                var validate = await _userManager.CreateAsync(user, request.UserDto.Password);

                if (validate.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userManager.AddToRoleAsync(user, "Guest");
                    return "User Created Sucessfully!";
                }

                return "Failed to Create User";
            }
            catch(Exception e)
            {
                return e.Message;
            }

        }

    }
}

