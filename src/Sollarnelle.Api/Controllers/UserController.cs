using Microsoft.AspNetCore.Mvc;
using Solarnelle.Api.Controllers.Base;
using Solarnelle.Application.Models.Request.Auth;
using Solarnelle.Application.Services.Auth;

namespace Solarnelle.Api.Controllers
{
    [Route("[controller]")]
    public class UserController(IAuthService authService) : SolarnelleBaseController
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpRequest request)
        {
            return Ok(await authService.SignUpAsync(request));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            return Ok(await authService.SignInAsync(request));
        }
    }
}
