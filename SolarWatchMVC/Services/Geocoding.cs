using System.Net;
using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Services;

public class Geocoding : IGeocoding
{
  private readonly ILogger<Geocoding> _logger;

  public Geocoding(ILogger<Geocoding> logger)
  {
    _logger = logger;
  }
  public async Task<string> GetGeocodeForCity(string cityName)
  {
    var apiKey = "f778716bc3508229848e8235af6fca4e";
    var url = $"http://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=5&appid={apiKey}";

    using var client = new HttpClient();
    
    _logger.LogInformation("Calling OpenWeather API Geocoding with url: {url}", url);
    
    var response = await client.GetAsync(url);
    var responseString = await response.Content.ReadAsStringAsync();
    
    using var document = JsonDocument.Parse(responseString);
    var root = document.RootElement;

   
    if (root.GetArrayLength() == 0)
    {
      _logger.LogError("OpenWeather API Geocoding: No valid data found for city '{cityName}'.", cityName);
      throw new ArgumentException("Invalid city name. Please provide a valid city name.", nameof(cityName)); 
    }

    return responseString; 
  }
}

