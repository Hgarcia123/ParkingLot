using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;


namespace Application.Methods.Authorization
{
    public abstract class AuthorizationAbstract
    {
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly SignInManager<IdentityUser> _signInManager; 

        public AuthorizationAbstract(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

    }
}
