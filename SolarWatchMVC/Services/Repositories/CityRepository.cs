using Microsoft.EntityFrameworkCore;
using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.Services.Repositories;

public class CityRepository : ICityRepository
{
    private SolarApiContext dbContext;
    public CityRepository(SolarApiContext context)
    {
        dbContext = context;
    }
    public City? GetByName(string name)
    {
        return dbContext.Cities.FirstOrDefault(c => c.Name == name);
    }

    public void Add(City city)
    {
        var existingCity = dbContext.Cities.FirstOrDefault(c => c.Name == city.Name);
        if (existingCity != null)
        {
            Console.WriteLine($"City {city.Name} already exists in the database.");
            return; 
        }

        Console.WriteLine($"Adding {city.Name} to database...");
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public async Task Update(City city)
    {
        var cityDbRepresentation = await dbContext.Cities.FirstOrDefaultAsync(data => data.Id == city.Id);
        
        if (cityDbRepresentation == null)
        {
            throw new InvalidOperationException($"City with ID {city.Id} not found.");
        }

        cityDbRepresentation.Name = city.Name;
        cityDbRepresentation.Country = city.Country;
        cityDbRepresentation.State = city.State;
        cityDbRepresentation.Latitude = city.Latitude;
        cityDbRepresentation.Longitude = city.Longitude;

        await dbContext.SaveChangesAsync();
    }
    
    public async Task Delete(int id)
    {
        var dbRepresentation = await dbContext.Cities.FirstOrDefaultAsync(data => data.Id == id);
        
        if (dbRepresentation == null)
        {
            throw new InvalidOperationException($"City with ID {id} not found.");
        }

        dbContext.Remove(dbRepresentation);
        await dbContext.SaveChangesAsync();
    }
}