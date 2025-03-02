using FunDooNotesC_.BusinessLayer; // Business Layer ko include kar raha hai
using Microsoft.AspNetCore.Mvc; // API controller banane ke liye
using FunDooNotesC_.BusinessLayer.Models; // Models ko access karne ke liye
using FunDooNotesC_.BusinessLayer.Models; // Yeh line duplicate hai, isko hata sakte hain

namespace FunDooNotesC_.BusinessLayer.Controllers // Namespace define kar raha hai jo is controller ko identify karega
{
    // Ye controller weather forecast se related API requests handle karega
    [ApiController] // Controller ko API controller ke roop me mark kar raha hai
    [Route("[controller]")] // API ka base route set kar raha hai, e.g., /WeatherForecast
    public class WeatherForecastController : ControllerBase // ControllerBase inherit kar raha hai
    {
        // Possible weather summaries ka ek list banaya hai
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger; // Logging ke liye private field banaya hai

        // Constructor jo logger ko inject karega
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger; // Logger ko assign kar raha hai private field me
        }

        // Get API (random weather forecast generate karne ke liye)
        [HttpGet(Name = "GetWeatherForecast")] // HTTP GET request ko handle karega
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5) // 1 se lekar 5 tak ek range bana raha hai
            .Select(index => new WeatherForecast // Har ek index ke liye ek naya WeatherForecast object create kar raha hai
            {
                Date = DateTime.Now.AddDays(index), // Aaj ki date se aage ke 5 din ka forecast generate kar raha hai
                TemperatureC = Random.Shared.Next(-20, 55), // Random temperature generate kar raha hai (-20 se 55 degree tak)
                Summary = Summaries[Random.Shared.Next(Summaries.Length)] // Summaries list me se randomly ek summary pick kar raha hai
            })
            .ToArray(); // Result ko array me convert kar raha hai
        }
    }
}
