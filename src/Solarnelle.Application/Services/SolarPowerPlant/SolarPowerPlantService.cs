using Microsoft.Extensions.Logging;
using Solarnelle.Application.Mappers;
using Solarnelle.Application.Models.Request.SolarPowerPlant;
using Solarnelle.Application.Models.Response.SolarPowerPlant;
using Solarnelle.Domain.Exceptions;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.Commands;

namespace Solarnelle.Application.Services.SolarPowerPlant
{
    public class SolarPowerPlantService(
        ISolarPowerPlantRepository solarPowerPlantRepository,
        ILogger<SolarPowerPlantService> logger) : ISolarPowerPlantService
    {
        public async Task AddAsync(Guid userId, AddSolarPowerPlantRequest request)
        {
            if (request.DateOfInstallation < DateTime.UtcNow)
                throw new CustomHttpException("Date of installation cannot be in the past. Please enter a valid date.", System.Net.HttpStatusCode.BadRequest);

            AddSolarPowerPlantCommand command = request.MapToCommand(userId);

            await solarPowerPlantRepository.AddAsync(command);
        }

        public async Task<GetSolarPowerPlantsResponse> GetAsync(GetSolarPowerPlantsRequest request)
        {
            FilterSolarPowerPlantCommand command = request.MapToCommand();

            var filteredSolarPowerPlants = await solarPowerPlantRepository.GetByFilterAsync(command);

            var response = new GetSolarPowerPlantsResponse()
            {
                SolarPowerPlants = filteredSolarPowerPlants.ToList().Select(s => s.MapToDto())
            };

            return response;
        }

        public async Task<GetSolarPowerPlantByIdResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var solarPowerPlant = await solarPowerPlantRepository.GetByIdAsync(id);

                return solarPowerPlant!.MapToResponse();
            }
            catch (DatabaseException databaseException)
            {
                logger.LogError(databaseException.Message, id);

                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, id);

                throw;
            }
        }

        public async Task UpdateAsync(Guid id, UpdateSolarPowerPlantRequest request)
        {
            UpdateSolarPowerPlantCommand command = request.MapToCommand();

            await solarPowerPlantRepository.UpdateAsync(id, command);
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await solarPowerPlantRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, id);

                throw;
            }
        }
    }
}
