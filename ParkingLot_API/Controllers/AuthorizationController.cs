using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

using Application.DtoModels;
using Application.Methods.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace ParkingLot_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerAbstract
    {
        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> RegisterUser([FromBody] CreateUserDto request)
        {
            return new JsonResult(await Mediator.Send(new RegisterUserRequest(request)));
        }

        [HttpPost]
        [Route("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto request)
        {
            var response = await Mediator.Send(new LoginUserRequest(request));

            if(response == "User login successful")
            {
                var accessToken = GenerateToken();
                SetJWTCookie(accessToken);

                return new JsonResult(accessToken);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("LogoutUser")]
        public async Task<IActionResult> LogoutUser()
        {
            return new JsonResult(await Mediator.Send(new LogoutUserRequest()));
        }

        private string GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MinhachaveTokenDefault"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void SetJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddMinutes(10),
            };
            Response.Cookies.Append("jwtCookie", token, cookieOptions);
        }
    }
}
