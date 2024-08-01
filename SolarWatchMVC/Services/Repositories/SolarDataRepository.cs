using Microsoft.EntityFrameworkCore;
using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.Services.Repositories;

public class SolarDataRepository : ISolarDataRepository
{
    private SolarApiContext dbContext;
    
    public SolarDataRepository(SolarApiContext context)
    {
        dbContext = context;
    }
    public SolarData? GetSolarData(int cityId, DateTime? date, string timeZone)
    {
            if (date == null)
            {
                date = DateTime.Today;
            }

            // Define the start and end of the day for the date
            var startOfDay = date.Value.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            return dbContext.SolarDatas
                .FirstOrDefault(data => data.Sunrise >= startOfDay && data.Sunrise <= endOfDay 
                                                                   && data.CityId == cityId 
                                                                   && data.TimeZone == timeZone);

    }
    
    public void Add(SolarData data)
    {
        dbContext.Add(data);
        dbContext.SaveChanges();
    }

    public async Task Update(SolarData newData)
    {
        var dbRepresentation = await dbContext.SolarDatas.FirstOrDefaultAsync(dataInDb => newData.Id == dataInDb.Id);
        
        if (dbRepresentation == null)
        {
            throw new InvalidOperationException($"Product with ID {newData.Id} not found.");
        }
        
        dbRepresentation.Sunrise = newData.Sunrise;
        dbRepresentation.Sunset = newData.Sunset;
        dbRepresentation.CityId = newData.CityId;
        dbRepresentation.TimeZone = newData.TimeZone;

        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var dbRepresentation = await dbContext.SolarDatas.FirstOrDefaultAsync(data => data.Id == id);
        
        if (dbRepresentation == null)
        {
            throw new InvalidOperationException($"Product with ID {id} not found.");
        }

        dbContext.Remove(dbRepresentation);
        await dbContext.SaveChangesAsync();
    }
    
}