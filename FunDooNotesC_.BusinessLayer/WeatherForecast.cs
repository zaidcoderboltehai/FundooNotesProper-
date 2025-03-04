namespace FunDooNotesC_.BusinessLayer.Models // Yeh namespace define kar raha hai jisme yeh model class exist karegi
{
    // Namespace ek container hai jisme related classes, models, ya code organize kiya jata hai.
    // Is case mein, `FunDooNotesC_.BusinessLayer.Models` namespace mein `WeatherForecast` class rakhi gayi hai.

    public class WeatherForecast // WeatherForecast naam ki ek class bana raha hai jo ek model hai
    {
        // `WeatherForecast` class ek model hai jo weather-related data ko represent karti hai.
        // Models ka use hota hai data ko structure karne ke liye.

        public DateTime Date { get; set; } // Yeh property weather forecast ka date store karegi
        // `Date` ek property hai jo `DateTime` type ki hai. Isme weather forecast ka date store hota hai.
        // `get; set;` ka matlab hai ki is property ko read aur write kiya ja sakta hai.

        public int TemperatureC { get; set; } // Yeh property temperature ko Celsius (°C) me store karegi
        // `TemperatureC` ek property hai jo temperature ko Celsius (°C) mein store karti hai.
        // `int` ka matlab hai ki yeh property integer values store karegi.

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556); // Yeh ek calculated property hai jo Celsius ko Fahrenheit me convert karegi
        // `TemperatureF` ek read-only property hai jo Celsius (`TemperatureC`) ko Fahrenheit (°F) mein convert karti hai.
        // Yeh property directly calculate hoti hai aur koi extra storage use nahi karti.

        public string? Summary { get; set; } // Yeh property ek optional string hai jo weather ka summary (e.g. "Cool", "Hot") store karegi
        // `Summary` ek property hai jo weather ka description store karti hai, jaise "Cool", "Hot", etc.
        // `string?` ka matlab hai ki yeh property optional hai (null ho sakti hai).
    }
}