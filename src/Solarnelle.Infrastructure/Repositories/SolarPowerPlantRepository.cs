using Microsoft.EntityFrameworkCore;
using Solarnelle.Domain.Exceptions;
using Solarnelle.Domain.Interfaces.DatabaseContext;
using Solarnelle.Domain.Interfaces.Repositories;
using Solarnelle.Domain.Models.Commands.SolarPowerPlant;
using Solarnelle.Domain.Models.Tables;

namespace Solarnelle.Infrastructure.Repositories
{
    public class SolarPowerPlantRepository(ISolarnelleDbContext solarnelleDbContext) : ISolarPowerPlantRepository
    {
        public async Task AddAsync(AddSolarPowerPlantCommand command)
        {
            SolarPowerPlant solarPowerPlant = new()
            {
                DateCreated = command.DateCreated,
                DateLastModified = command.DateLastModified,
                InstalledPower = command.InstalledPower,
                DateOfInstallation = command.DateOfInstallation,
                Latitude = command.Latitude,
                Longitude = command.Longitude,
                Name = command.Name
            };

            await solarnelleDbContext.SolarPowerPlants.AddAsync(solarPowerPlant);

            await solarnelleDbContext.SaveChangesAsync();
        }

        public async Task<SolarPowerPlant?> GetByIdAsync(Guid id)
        {
            var solarPowerPlant = await solarnelleDbContext.SolarPowerPlants.FindAsync(id);

            if (solarPowerPlant == null)
                throw new DatabaseException($"No {nameof(SolarPowerPlant)} entity found with {nameof(id)}: {id}.", nameof(GetByIdAsync), nameof(SolarPowerPlantRepository));

            return solarPowerPlant;
        }

        public async Task<List<SolarPowerPlant>> GetByFilterAsync(FilterSolarPowerPlantCommand command)
        {
            var solarPowerPlants = solarnelleDbContext.SolarPowerPlants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(command.Name))
                solarPowerPlants = solarPowerPlants.Where(s => !string.IsNullOrWhiteSpace(s.Name) && s.Name.Contains(command.Name));

            if (command.DateOfInstallation.HasValue)
            {
                var date = command.DateOfInstallation.Value.Date;

                solarPowerPlants = solarPowerPlants.Where(s => s.DateOfInstallation.Date == date);
            }

            if (command.Latitude.HasValue && command.Longitude.HasValue)
                solarPowerPlants = solarnelleDbContext.SolarPowerPlants.Where(s => s.Latitude == command.Latitude.Value && s.Longitude == command.Longitude.Value);

            return await solarPowerPlants.ToListAsync();
        }

        public async Task UpdateAsync(Guid id, UpdateSolarPowerPlantCommand command)
        {
            var solarPowerPlant = await solarnelleDbContext.SolarPowerPlants.FindAsync(id) ?? 
                throw new DatabaseException($"No {nameof(SolarPowerPlant)} entity found with {nameof(id)}: {id}.", nameof(UpdateAsync), nameof(SolarPowerPlantRepository));
            
            solarPowerPlant.Name = command.Name;
            solarPowerPlant.DateOfInstallation = command.DateOfInstallation;
            solarPowerPlant.DateLastModified = command.DateLastModified;
            solarPowerPlant.Longitude = command.Longitude;
            solarPowerPlant.Latitude = command.Latitude;

            await solarnelleDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var powerPlantForDeletion = await solarnelleDbContext.SolarPowerPlants.SingleAsync(x => x.Id == id);

            solarnelleDbContext.SolarPowerPlants.Remove(powerPlantForDeletion);

            await solarnelleDbContext.SaveChangesAsync();
        }
    }
}
