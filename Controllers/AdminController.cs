using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReserveServer.ReserveModel;
using ReserveServer.DTO;

namespace ReserveServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<ReservationCustomerUser> userManager,
        JWTHandler jwtHandler) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            ReservationCustomerUser? user = await userManager.FindByNameAsync(loginRequest.userName);
            if (user == null)
            {
                return Unauthorized("Bad username");
            }

            bool success = await userManager.CheckPasswordAsync(user, loginRequest.password);
            if (!success)
            {
                return Unauthorized("Bad password");
            }

            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            var jwtString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "Mom loves me",
                Token = jwtString,
            });
        }
    }

}
