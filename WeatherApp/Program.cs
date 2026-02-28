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

form.Load += async (_, _) =>
{
    try
    {
        using var service = new WeatherService(config);
        var weather = await service.GetCurrentWeatherAsync();
        form.ShowWeather(weather);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("ApiKey"))
    {
        form.ShowError("API key not configured.\n\nOpen appsettings.json and set your key.");
    }
    catch (HttpRequestException ex)
    {
        form.ShowError($"Network error:\n\n{ex.Message}");
    }
    catch (Exception ex)
    {
        form.ShowError($"Unexpected error:\n\n{ex.Message}");
    }
};

Application.Run(form);
