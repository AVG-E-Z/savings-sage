using Microsoft.AspNetCore.Mvc;

using SolarWatchMVC.ViewModels; 


namespace SolarWatchMVC.Controllers
{
    public class SolarWatchController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SolarWatchController> _logger;

        public SolarWatchController(IHttpClientFactory httpClientFactory, ILogger<SolarWatchController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("SolarWatch")]
        public IActionResult SolarWatch()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var viewModel = new SolarWatchViewModel();
            return View(viewModel);
        }

        [HttpPost("/SolarWatch/FetchData")]
        public async Task<IActionResult> FetchData(SolarWatchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SolarWatch", model);
            }

            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            
            var apiBaseUrl = "http://localhost:5174/"; 
            string url;

            if (model.SunData == "Sunset")
            {
                url =
                    $"{apiBaseUrl}SunriseSunset/GetSunset?city={Uri.EscapeDataString(model.City)}&timeZone={Uri.EscapeDataString("Europe/Budapest")}";
                model.TypeOfSunData = "Sunset";
            }
            else
            {
                url =
                    $"{apiBaseUrl}SunriseSunset/GetSunrise?city={Uri.EscapeDataString(model.City)}&timeZone={Uri.EscapeDataString("Europe/Budapest")}";
                model.TypeOfSunData = "Sunrise";
            }

            if (model.Date.HasValue)
            {
                url += $"&date={Uri.EscapeDataString(model.Date.Value.ToString("yyyy-MM-dd"))}";
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    model.DataToDisplay = await response.Content.ReadAsStringAsync();
                    model.CityToDisplay = model.City;
                    model.IsDisplayed = true;
                }
                else
                {
                    model.IsSomethingWrong = true;
                    _logger.LogWarning("API request failed with status code: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching solar data.");
                model.IsSomethingWrong = true;
            }

            return View("SolarWatch", model);
        }
    }
}
