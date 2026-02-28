using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using WeatherApp.Models;

namespace WeatherApp.Services;

public class WeatherService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _city;
    private readonly string _state;
    private readonly string _countryCode;
    private readonly string _units;

    public WeatherService(IConfiguration config)
    {
        _httpClient = new HttpClient();
        _apiKey      = config["WeatherApi:ApiKey"]      ?? throw new InvalidOperationException("WeatherApi:ApiKey is missing from appsettings.json");
        _city        = config["WeatherApi:City"]        ?? "Grand Prairie";
        _state       = config["WeatherApi:State"]       ?? "TX";
        _countryCode = config["WeatherApi:CountryCode"] ?? "US";
        _units       = config["WeatherApi:Units"]       ?? "imperial";
    }

    public async Task<WeatherResponse> GetCurrentWeatherAsync()
    {
        string query = $"{_city},{_state},{_countryCode}";
        string url   = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(query)}&appid={_apiKey}&units={_units}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            string body = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"OpenWeatherMap API returned {(int)response.StatusCode} {response.ReasonPhrase}.\n{body}");
        }

        var weather = await response.Content.ReadFromJsonAsync<WeatherResponse>()
            ?? throw new InvalidOperationException("Received empty response from weather API.");

        return weather;
    }

    public void Dispose() => _httpClient.Dispose();
}
