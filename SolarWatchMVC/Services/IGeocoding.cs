namespace SolarWatch.Services;

public interface IGeocoding
{
    public Task<string> GetGeocodeForCity(string cityName);
}