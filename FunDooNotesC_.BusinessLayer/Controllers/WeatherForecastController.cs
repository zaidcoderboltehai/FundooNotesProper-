using FunDooNotesC_.BusinessLayer; // Business Layer ko include kar raha hai, taaki uske classes aur methods use kar sakein.
using Microsoft.AspNetCore.Mvc; // API controller banane ke liye necessary classes aur methods include kar raha hai.
using FunDooNotesC_.BusinessLayer.Models; // Models (jaise WeatherForecast) ko access karne ke liye include kiya gaya hai.
using FunDooNotesC_.BusinessLayer.Models; // Yeh line duplicate hai, isko hata sakte hain (extra hai, zaroorat nahi).

namespace FunDooNotesC_.BusinessLayer.Controllers // Namespace define kar raha hai, jo is controller ko identify karega.
{
    // Ye controller weather forecast se related API requests handle karega.
    [ApiController] // Ye attribute batata hai ki ye class ek API controller hai.
    [Route("[controller]")] // Ye attribute API ka base route set kar raha hai.
    // [controller] ka matlab hai ki route mein controller ka name (WeatherForecast) use hoga.
    // Iska matlab ye API endpoint `/WeatherForecast` pe available hoga.
    public class WeatherForecastController : ControllerBase // Ye class ControllerBase se inherit karti hai.
    {
        // Possible weather summaries ka ek list banaya hai.
        // Yeh list weather ke descriptions ko store karta hai.
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger; // Logging ke liye private field banaya hai.
        // ILogger ka use application ke logs create karne ke liye hota hai.

        // Constructor jo logger ko inject karega.
        // Dependency injection ke through logger ka instance milega.
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger; // Logger ko assign kar raha hai private field mein.
        }

        // Get API (random weather forecast generate karne ke liye).
        [HttpGet(Name = "GetWeatherForecast")] // Ye attribute batata hai ki ye method HTTP GET request ko handle karega.
        // Name = "GetWeatherForecast" is method ka naam define karta hai, jo Swagger documentation mein dikhega.
        public IEnumerable<WeatherForecast> Get()
        {
            // Enumerable.Range(1, 5) se 1 se 5 tak ek range generate kiya jata hai.
            // Iska matlab hai ki 5 weather forecasts generate kiye jaayenge.
            return Enumerable.Range(1, 5)
            // .Select() ka use har ek number (index) ke liye ek naya WeatherForecast object create karne ke liye hota hai.
            .Select(index => new WeatherForecast
            {
                // Date property mein aaj ki date se aage ke 5 din ka date set kiya jata hai.
                Date = DateTime.Now.AddDays(index),
                // TemperatureC property mein random temperature generate kiya jata hai (-20 se 55 degree Celsius tak).
                TemperatureC = Random.Shared.Next(-20, 55),
                // Summary property mein Summaries list se randomly ek description pick kiya jata hai.
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            // .ToArray() ka use result ko array mein convert karne ke liye hota hai.
            .ToArray();
        }
    }
}