using WeatherApp.Models;

namespace WeatherApp;

public class WeatherForm : Form
{
    private readonly Label _weatherLabel;

    public WeatherForm()
    {
        Text = "WeatherApp — Grand Prairie, TX";
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        MinimumSize = new Size(340, 0);
        Padding = new Padding(24);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        BackColor = Color.FromArgb(30, 30, 40);

        _weatherLabel = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 12f, FontStyle.Regular),
            ForeColor = Color.WhiteSmoke,
            Padding = new Padding(8),
        };

        Controls.Add(_weatherLabel);
    }

    public void ShowLoading()
    {
        _weatherLabel.Text = "Fetching weather...";
    }

    public void ShowWeather(WeatherResponse weather)
    {
        string city       = weather.CityName ?? "Unknown";
        double temp       = weather.Main?.Temp      ?? 0;
        double feelsLike  = weather.Main?.FeelsLike ?? 0;
        int    humidity   = weather.Main?.Humidity  ?? 0;
        double windSpeed  = weather.Wind?.Speed     ?? 0;
        string conditions = weather.Weather?[0]?.Description ?? "Unknown";
        conditions = conditions.Length > 0
            ? char.ToUpper(conditions[0]) + conditions[1..]
            : conditions;

        _weatherLabel.Text =
            $"{city}, TX\n" +
            $"─────────────────────────\n" +
            $"Conditions  :  {conditions}\n" +
            $"Temperature :  {temp:F1} °F\n" +
            $"Feels Like  :  {feelsLike:F1} °F\n" +
            $"Humidity    :  {humidity}%\n" +
            $"Wind Speed  :  {windSpeed:F1} mph";
    }

    public void ShowError(string message)
    {
        _weatherLabel.ForeColor = Color.Salmon;
        _weatherLabel.Text = message;
    }
}
