using System.Net;
using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Services;

public class SunriseSunsetApi : ISolarApi
{
    private readonly ILogger<SunriseSunsetApi> _logger;
    public SunriseSunsetApi(ILogger<SunriseSunsetApi> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetSunriseAndSunset(Coordinate coordinate, string timeZone, DateTime? date)
    {
        if (coordinate.Latitude < -90 || coordinate.Latitude > 90 || coordinate.Longitude < -180 || coordinate.Longitude > 180)
        {
            throw new ArgumentException("Invalid input data. Please check coordinates and timezone.");
        }

        var url = generateURL(coordinate, timeZone, date);
        using var client = new HttpClient();
        HttpResponseMessage response;
        JsonDocument document;

        try
        {
            _logger.LogInformation("Calling Sunrise-Sunset API with url: {url}", url);
           
            response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            document = JsonDocument.Parse(responseString);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during Sunrise-Sunset API call.");
            throw;
        }
        
        var root = document.RootElement;
        
        if (!root.EnumerateObject().Any())
        {
            _logger.LogError("Solar API: No valid data found for coordinates '{coordinate.Latitude}, {coordinate.Longitude}', and timezone: {timeZone}" );
            throw new ArgumentException("Invalid input data. Please check coordinates and timezone."); 
        }

        return await response.Content.ReadAsStringAsync();
    }
    

    private string generateURL(Coordinate coordinate, string timeZone, DateTime? date)
    {
        var dateString = date?.ToString("yyyy-MM-dd") ?? "today"; // Format date or use "today" if null
        return $"https://api.sunrise-sunset.org/json?lat={coordinate.Latitude}&lng={coordinate.Longitude}&date={dateString}&tzid={timeZone}&formatted=0";
    }
}