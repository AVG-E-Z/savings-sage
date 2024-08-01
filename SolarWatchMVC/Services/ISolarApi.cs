using SolarWatch.Model;

namespace SolarWatch.Services;

public interface ISolarApi
{
    public Task<string> GetSunriseAndSunset(Coordinate coordinate, string timeZone, DateTime? date);
}