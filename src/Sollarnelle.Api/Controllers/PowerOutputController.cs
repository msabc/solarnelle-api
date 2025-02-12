using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarnelle.Api.Controllers.Base;
using Solarnelle.Application.Constants;
using Solarnelle.Application.Models.Request.PowerOutput;
using Solarnelle.Application.Models.Response.PowerOutput;
using Solarnelle.Application.Services.PowerOutput;

namespace Solarnelle.Api.Controllers
{
    [Route("power-output")]
    [Authorize(Policy = SecurityPolicies.SolarnelleUserIdPolicyName)]
    public class PowerOutputController(IPowerOutputService powerOutputService) : SolarnelleBaseController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetPowerOutputResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAsync([FromQuery] GetPowerOutputRequest request)
        {
            var response = await powerOutputService.GetTimeseriesAsync(request);

            if (!response.Any())
            {
                return NoContent();
            }

            return Ok(response);
        }
    }
}
