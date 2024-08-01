using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public DateTime GetSunrise(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement sunrise = results.GetProperty("sunrise");
        
        return sunrise.GetDateTime();
    }

    public DateTime GetSunset(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        JsonElement sunset = results.GetProperty("sunset");
        
        return sunset.GetDateTime();
    }
    
    public Coordinate ConvertDataToCoordinate(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement lat = json.RootElement[0].GetProperty("lat");
        JsonElement lon = json.RootElement[0].GetProperty("lon");

        return new Coordinate(lat.GetDouble(), lon.GetDouble());
    }

    public City ConvertDataToCity(string data)
    {
        Coordinate coordinate = ConvertDataToCoordinate(data);
        JsonDocument json = JsonDocument.Parse(data);
        string name = json.RootElement[0].GetProperty("name").GetString();
        string country = json.RootElement[0].GetProperty("country").GetString();
        string? state = json.RootElement[0].TryGetProperty("state", out JsonElement stateElement) ? stateElement.GetString() : null;

        return new City(name, coordinate.Latitude, coordinate.Longitude, country, state);
    }
    
}