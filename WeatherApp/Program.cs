using Microsoft.Extensions.Configuration;
using WeatherApp.Services;

// Build configuration from appsettings.json
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

try
{
    using var service = new WeatherService(config);
    var weather = await service.GetCurrentWeatherAsync();

    string city       = weather.CityName ?? "Unknown";
    double temp       = weather.Main?.Temp      ?? 0;
    double feelsLike  = weather.Main?.FeelsLike ?? 0;
    int    humidity   = weather.Main?.Humidity  ?? 0;
    double windSpeed  = weather.Wind?.Speed     ?? 0;
    string conditions = weather.Weather?[0]?.Description ?? "Unknown";
    // Capitalize first letter of conditions
    conditions = conditions.Length > 0
        ? char.ToUpper(conditions[0]) + conditions[1..]
        : conditions;

    string message =
        $"Current Weather — {city}, TX\n" +
        $"─────────────────────────────\n" +
        $"Conditions  : {conditions}\n" +
        $"Temperature : {temp:F1} °F\n" +
        $"Feels Like  : {feelsLike:F1} °F\n" +
        $"Humidity    : {humidity}%\n" +
        $"Wind Speed  : {windSpeed:F1} mph";

    MessageBox.Show(message, "WeatherApp — Grand Prairie, TX",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
}
catch (InvalidOperationException ex) when (ex.Message.Contains("ApiKey"))
{
    MessageBox.Show(
        "API key not configured.\n\nPlease open appsettings.json and replace\nYOUR_OPENWEATHERMAP_API_KEY_HERE with your real key.",
        "WeatherApp — Configuration Error",
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
catch (HttpRequestException ex)
{
    MessageBox.Show(
        $"Failed to retrieve weather data:\n\n{ex.Message}",
        "WeatherApp — Network Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
catch (Exception ex)
{
    MessageBox.Show(
        $"An unexpected error occurred:\n\n{ex.Message}",
        "WeatherApp — Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
}
