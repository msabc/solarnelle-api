using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarnelle.Api.Controllers.Base;
using Solarnelle.Application.Constants;
using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Services.Security;
using Solarnelle.Application.Services.SolarPowerPlant;

namespace Solarnelle.Api.Controllers
{
    [Route("power-plant")]
    [Authorize(Policy = SecurityPolicies.SolarnelleUserIdPolicyName)]
    public class SolarPowerPlantController(
        ICurrentUserService currentUserService, 
        ISolarPowerPlantService solarPowerPlantService) : SolarnelleBaseController
    {
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddSolarPowerPlantRequest request)
        {
            Guid currentUserId = currentUserService.ResolveCurrentUserId(User);

            await solarPowerPlantService.AddAsync(currentUserId, request);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromQuery] GetSolarPowerPlantsRequest request)
        {
            return Ok(await solarPowerPlantService.GetAsync(request));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetByIdAsync([FromQuery] Guid id)
        {
            var response = await solarPowerPlantService.GetByIdAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSolarPowerPlantRequest request)
        {
            Guid currentUserId = currentUserService.ResolveCurrentUserId(User);

            await solarPowerPlantService.UpdateAsync(currentUserId, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await solarPowerPlantService.DeleteAsync(id);

            return NoContent();
        }
    }
}
