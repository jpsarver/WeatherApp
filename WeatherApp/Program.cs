using Microsoft.Extensions.Configuration;
using WeatherApp;
using WeatherApp.Services;

ApplicationConfiguration.Initialize();

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

var form = new WeatherForm();
form.ShowLoading();

async Task FetchWeatherAsync()
{
    form.ShowLoading();
    try
    {
        using var service = new WeatherService(config);
        var weather = await service.GetCurrentWeatherAsync();
        form.ShowWeather(weather);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("ApiKey"))
    {
        form.ShowError("API key not configured.\nOpen appsettings.json and set your key.");
    }
    catch (HttpRequestException ex)
    {
        form.ShowError($"Network error:\n{ex.Message}");
    }
    catch (Exception ex)
    {
        form.ShowError($"Unexpected error:\n{ex.Message}");
    }
}

form.Load             += async (_, _) => await FetchWeatherAsync();
form.RefreshRequested += async (_, _) => await FetchWeatherAsync();

Application.Run(form);
