using SolarWatch.Model;

namespace SolarWatch.Services.Repositories;

public interface ISolarDataRepository
{
    public SolarData? GetSolarData(int cityId, DateTime? date, string TimeZone);

    public void Add(SolarData data);

    public Task Update(SolarData newData);

    public Task Delete(int id);
}