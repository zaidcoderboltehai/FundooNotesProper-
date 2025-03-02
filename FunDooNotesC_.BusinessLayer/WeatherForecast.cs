namespace FunDooNotesC_.BusinessLayer.Models // Yeh namespace define kar raha hai jisme yeh model class exist karegi
{
    public class WeatherForecast // WeatherForecast naam ki ek class bana raha hai jo ek model hai
    {
        public DateTime Date { get; set; } // Yeh property weather forecast ka date store karegi

        public int TemperatureC { get; set; } // Yeh property temperature ko Celsius (°C) me store karegi

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // Yeh ek calculated property hai jo Celsius ko Fahrenheit me convert karegi

        public string? Summary { get; set; } // Yeh property ek optional string hai jo weather ka summary (e.g. "Cool", "Hot") store karegi
    }
}
