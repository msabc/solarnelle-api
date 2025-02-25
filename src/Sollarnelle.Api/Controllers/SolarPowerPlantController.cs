using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solarnelle.Api.Controllers.Base;
using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Services.Security;
using Solarnelle.Application.Services.SolarPowerPlant;

namespace Solarnelle.Api.Controllers
{
    [Route("power-plant")]
    public class SolarPowerPlantController(ISolarPowerPlantService solarPowerPlantService) : SolarnelleBaseController
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] AddSolarPowerPlantRequest request)
        {
            await solarPowerPlantService.AddAsync(request);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetSolarPowerPlantsRequest request)
        {
            return Ok(await solarPowerPlantService.GetAsync(request));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await solarPowerPlantService.GetByIdAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSolarPowerPlantRequest request)
        {
            Guid currentUserId = currentUserService.ResolveCurrentUserId(User);

            await solarPowerPlantService.UpdateAsync(currentUserId, request);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await solarPowerPlantService.DeleteAsync(id);

            return NoContent();
        }
    }
}
