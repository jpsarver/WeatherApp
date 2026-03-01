# WeatherApp

A Windows desktop application that displays current weather conditions for **Grand Prairie, TX** using the [OpenWeatherMap API](https://openweathermap.org/api). Built with C# .NET and Windows Forms.

---

## Prerequisites

| Requirement | Version |
|-------------|---------|
| [.NET SDK](https://dotnet.microsoft.com/download) | 10.0 or later |
| Windows 10 / 11 | x64 |
| OpenWeatherMap API key | Free tier |

> **Get a free API key:** Register at [https://openweathermap.org/api](https://openweathermap.org/api) and generate a key under *API keys* in your account dashboard.

---

## Setup

### 1. Clone the repository

```bash
git clone https://github.com/jpsarver/WeatherApp.git
cd WeatherApp
```

### 2. Configure your API key

Open `WeatherApp/appsettings.json` and set your key:

```json
{
  "WeatherApi": {
    "ApiKey": "YOUR_OPENWEATHERMAP_API_KEY_HERE",
    "City": "Grand Prairie",
    "State": "TX",
    "CountryCode": "US",
    "Units": "imperial"
  }
}
```

| Setting | Description |
|---------|-------------|
| `ApiKey` | Your OpenWeatherMap API key |
| `City` | City name |
| `State` | Two-letter US state code |
| `CountryCode` | ISO 3166 country code |
| `Units` | `imperial` (°F / mph) or `metric` (°C / m/s) |

### 3. Restore and run

```bash
dotnet restore WeatherApp/WeatherApp.csproj
dotnet run --project WeatherApp/WeatherApp.csproj
```

---

## Features

- Current temperature, feels like, conditions, humidity, and wind speed
- Weather-condition-driven color theme (blue for clear, grey for clouds, dark for storms, etc.)
- Animated loading spinner while fetching data
- Refresh button to re-fetch live conditions
- Inline error display — no popups

---

## Project Structure

```
WeatherApp/
├── WeatherApp.slnx
├── .gitignore
├── README.md
├── SPEC.md
├── ARCHITECTURE.md
├── CHANGELOG.md
├── CONTRIBUTING.md
├── TODO.md
└── WeatherApp/
    ├── WeatherApp.csproj
    ├── appsettings.json
    ├── Program.cs
    ├── WeatherForm.cs
    ├── Models/
    │   └── WeatherResponse.cs
    └── Services/
        └── WeatherService.cs
```

---

## License

MIT
