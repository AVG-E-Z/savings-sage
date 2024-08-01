
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Services;
using SolarWatch.Services.Repositories;

namespace SolarWatchMVC.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly IGeocoding _geocoding;
    private readonly ISolarApi _sunriseSunsetApi;
    private readonly ICityRepository _cityRepository;
    private readonly ISolarDataRepository _solarDataRepository;

    public SunriseSunsetController(IJsonProcessor jsonProcessorForSunrise, ILogger<SunriseSunsetController> logger, IGeocoding geocoding, ISolarApi sunriseSunsetApi, ICityRepository cityRepository, ISolarDataRepository solarDataRepository)
    {
        _jsonProcessor = jsonProcessorForSunrise;
        _logger = logger;
        _geocoding = geocoding;
        _sunriseSunsetApi = sunriseSunsetApi;
        _cityRepository = cityRepository;
        _solarDataRepository = solarDataRepository;
    }

    [HttpGet("GetSunrise"), Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<DateTime>> GetSunrise([FromQuery, Required]string city, [FromQuery, Required]string timeZone, [FromQuery] DateTime? date = null)
    {
        var cityFromDb = _cityRepository.GetByName(city);

        if (cityFromDb != null)
        {
            var solarData = _solarDataRepository.GetSolarData(cityFromDb.Id, date, timeZone);

            if (solarData != null)
            {
                return Ok(solarData.Sunrise);
            }
        }
        try
        {
            DateTime? sunriseDate = date.HasValue ? date.Value : null;
            timeZone = Uri.UnescapeDataString(timeZone);
            var geocodingResponse = await _geocoding.GetGeocodeForCity(city);
            Coordinate coordinateForCity =
                _jsonProcessor.ConvertDataToCoordinate(geocodingResponse);
            var cityToAdd = _jsonProcessor.ConvertDataToCity(geocodingResponse);

            if (cityFromDb == null)
            {
                _cityRepository.Add(cityToAdd);
            }
            
            var sunriseSunsetData = await _sunriseSunsetApi.GetSunriseAndSunset(coordinateForCity, timeZone, sunriseDate);

            var sunrise = _jsonProcessor.GetSunrise(sunriseSunsetData);
            var sunset = _jsonProcessor.GetSunset(sunriseSunsetData);
            
           _solarDataRepository.Add(new SolarData(sunrise, sunset, cityToAdd.Id, timeZone)); 
            
            return Ok(sunrise);
        }
        catch (ArgumentException ae) //lehet saját ex típust is specifikálni
        {
            return BadRequest(ae.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunrise data");
            return NotFound("Error getting sunrise data");
        }
    }
    
    [HttpGet("GetSunset"),Authorize(Roles = "User, Admin")]
    public async Task<ActionResult<string>> GetSunset([FromQuery, Required]string city, [FromQuery, Required]string timeZone, [FromQuery]DateTime? date = null)
    {
        var cityFromDb = _cityRepository.GetByName(city);

        if (cityFromDb != null)
        {
            var solarData = _solarDataRepository.GetSolarData(cityFromDb.Id, date, timeZone);

            if (solarData != null)
            {
                return Ok(solarData.Sunset);
            }
        }
        try
        {
            timeZone = Uri.UnescapeDataString(timeZone);
            DateTime? sunsetDate = date.HasValue ? date.Value : null;
            var geocodingResponse = await _geocoding.GetGeocodeForCity(city);
            Coordinate coordinateForCity =
                _jsonProcessor.ConvertDataToCoordinate(geocodingResponse);
            
            var cityToAdd = _jsonProcessor.ConvertDataToCity(geocodingResponse);
            
            _cityRepository.Add(cityToAdd);

            var sunriseSunsetData = await _sunriseSunsetApi.GetSunriseAndSunset(coordinateForCity, timeZone, sunsetDate);
            
            var sunrise = _jsonProcessor.GetSunrise(sunriseSunsetData);
            var sunset = _jsonProcessor.GetSunset(sunriseSunsetData);
            
            _solarDataRepository.Add(new SolarData(sunrise, sunset, cityToAdd.Id, timeZone));
            
            return Ok(sunset);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunset data");
            return NotFound("Error getting sunset data");
        }
    }

    [HttpPut("UpdateSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarData>> UpdateSolarData([Required]int id, [Required]int  cityId, [Required]string timeZone, [Required]DateTime sunrise, [Required]DateTime sunset)
    {
        try
        {
            var newData = new SolarData(sunrise, sunset, cityId, timeZone);
            newData.Id = id;

            await _solarDataRepository.Update(newData);

            return Ok(newData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating Solar Data");
            return NotFound("Error updating Solar Data"); 
        }
    }
    
    [HttpPut("UpdateCity"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarData>> UpdateCity([Required]int id,  [Required]string country, [Required]string name, string? state, [Required]double latitude,[Required]double longitude)
    {
        try
        {
            var newData = new City(name, latitude, longitude, country, state)
            {
                Id = id
            };

            await _cityRepository.Update(newData);

            return Ok(newData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating City");
            return NotFound("Error updating City"); 
        }
    }

    [HttpDelete("DeleteSolarData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> DeleteSolarData(int id)
    {
        try
        {
            await _solarDataRepository.Delete(id);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting Solar Data");
            return NotFound("Error deleting Solar Data"); 
        }
    }
    
    [HttpDelete("DeleteCity"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<int>> DeleteCity([FromQuery]int id)
    {
        try
        {
            await _cityRepository.Delete(id);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting City");
            return NotFound("Error deleting City"); 
        }
    }
    
    [HttpPost("PostCityToDb"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarData>> PostCity([FromQuery, Required]string country, [FromQuery, Required]string name, [FromQuery] string? state, [FromQuery, Required]double latitude,[FromQuery, Required]double longitude)
    {
        try
        {
            var newData = new City(name, latitude, longitude, country, state);

             _cityRepository.Add(newData);

            return Ok(newData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding City");
            return NotFound("Error adding City"); 
        }
    }
    
    [HttpPost("PostSolarDataToDb"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<SolarData>> PostSolarData([Required]int  cityId, [Required]string timeZone, [Required]DateTime sunrise, [Required]DateTime sunset)
    {
        try
        {
            var newData = new SolarData(sunrise, sunset, cityId, timeZone);

           _solarDataRepository.Add(newData);

            return Ok(newData);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding Solar Data");
            return NotFound("Error adding Solar Data"); 
        }
    }
}