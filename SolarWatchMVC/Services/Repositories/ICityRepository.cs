using SolarWatch.Model;

namespace SolarWatch.Services.Repositories;

public interface ICityRepository
{
    public City? GetByName(string name);

    public void Add(City city);
    
    public Task Delete(int id);

    public Task Update(City city);
}
