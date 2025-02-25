using Microsoft.AspNetCore.Mvc;
using Solarnelle.Api.Controllers.Base;
using Solarnelle.Application.Models.Request.Auth;
using Solarnelle.Application.Services.User;

namespace Solarnelle.Api.Controllers
{
    [Route("[controller]")]
    public class UserController(IUserService userService) : SolarnelleBaseController
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            return Ok(await userService.SignUpAsync(request));
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            return Ok(await userService.SignInAsync(request));
        }
    }
}
