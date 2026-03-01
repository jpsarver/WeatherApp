# WeatherApp — Technical Architecture

## 1. Project Structure

```
WeatherApp/
├── WeatherApp.slnx                  # Visual Studio solution
├── .gitignore
└── WeatherApp/                      # Single project
    ├── WeatherApp.csproj            # Target: net10.0-windows, UseWindowsForms
    ├── appsettings.json             # Runtime configuration
    ├── Program.cs                   # Entry point — wires config, form, events
    ├── WeatherForm.cs               # All UI — layout, theming, public API
    ├── Models/
    │   └── WeatherResponse.cs       # JSON deserialization models
    └── Services/
        └── WeatherService.cs        # OpenWeatherMap HTTP client
```

---

## 2. Class Diagram

```
┌────────────────────────────────────────────────────────┐
│                        Program                         │
│  (top-level statements)                                │
│                                                        │
│  + IConfiguration config                               │
│  + WeatherForm form                                    │
│  + Task FetchWeatherAsync()                            │
│                                                        │
│  Wires: form.Load → FetchWeatherAsync()                │
│         form.RefreshRequested → FetchWeatherAsync()    │
└──────────────┬──────────────────────────┬─────────────┘
               │ creates                  │ creates
               ▼                          ▼
┌──────────────────────────┐  ┌────────────────────────────────────┐
│       WeatherForm        │  │          WeatherService             │
│  : Form                  │  │  : IDisposable                      │
│                          │  │                                     │
│  + ShowLoading()         │  │  - HttpClient _httpClient           │
│  + HideLoading()         │  │  - string _apiKey                   │
│  + ShowWeather(response) │  │  - string _city                     │
│  + ShowError(message)    │  │  - string _state                    │
│  + event RefreshRequested│  │  - string _countryCode              │
│                          │  │  - string _units                    │
│  - ApplyTheme(condition) │  │                                     │
│  - BuildHeader()         │  │  + GetCurrentWeatherAsync()         │
│  - BuildMainSection()    │  │    : Task<WeatherResponse>          │
│  - BuildDetailsSection() │  │  + Dispose()                        │
│  - BuildFooter()         │  └──────────────┬──────────────────────┘
│  - BuildLoadingOverlay() │                 │ returns
└──────────────────────────┘                 ▼
                               ┌─────────────────────────────┐
                               │       WeatherResponse        │
                               │                              │
                               │  + string? CityName          │
                               │  + List<WeatherCondition>?   │
                               │    Weather                   │
                               │  + MainData? Main            │
                               │  + WindData? Wind            │
                               └──────────────────────────────┘
                                          │ contains
                    ┌─────────────────────┼──────────────────────┐
                    ▼                     ▼                      ▼
        ┌────────────────────┐  ┌──────────────────┐  ┌──────────────────┐
        │  WeatherCondition  │  │     MainData     │  │     WindData     │
        │  + string? Main    │  │  + double Temp   │  │  + double Speed  │
        │  + string? Desc    │  │  + double Feels  │  └──────────────────┘
        └────────────────────┘  │  + int Humidity  │
                                └──────────────────┘
```

---

## 3. Data Flow

```
 Startup / Refresh button click
        │
        ▼
 FetchWeatherAsync()  [Program.cs]
        │
        ├──► form.ShowLoading()
        │         └── loading overlay visible, dot-timer starts
        │
        ▼
 WeatherService.GetCurrentWeatherAsync()
        │
        ├── Build URL from appsettings config
        ├── HttpClient.GetAsync(url)
        ├── Check response.IsSuccessStatusCode
        │       └── false → throw HttpRequestException
        └── ReadFromJsonAsync<WeatherResponse>()
                │
                ▼
        WeatherResponse (deserialized)
                │
                ▼
 form.ShowWeather(weather)
        │
        ├── HideLoading()  (stops timer, hides overlay)
        ├── Update all labels
        └── ApplyTheme(weather.main condition)
                ├── Set form BackColor
                ├── Set panel BackColors
                └── Set main icon emoji

 On any exception:
        └── form.ShowError(message)
                ├── HideLoading()
                ├── Hide details panel
                └── Show red error panel with message
```

---

## 4. Threading Model

All UI updates run on the UI thread. `WeatherService.GetCurrentWeatherAsync()` is awaited with `async/await` from the `form.Load` and `form.RefreshRequested` event handlers. The `InvokeRequired` / `Invoke()` guards in `WeatherForm`'s public methods ensure thread safety if ever called from a background thread.

---

## 5. Configuration

`appsettings.json` is loaded at startup via `Microsoft.Extensions.Configuration`:

```csharp
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();
```

The file is copied to the output directory on every build (`CopyToOutputDirectory: Always`).

---

## 6. Dependencies

### NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.Extensions.Configuration` | 8.0.0 | Configuration abstraction |
| `Microsoft.Extensions.Configuration.Json` | 8.0.0 | `appsettings.json` provider |

### Framework

| Component | Source |
|-----------|--------|
| `System.Net.Http.Json` | Built into .NET 6+ |
| `System.Windows.Forms` | Enabled via `<UseWindowsForms>true</UseWindowsForms>` |
| `System.Text.Json` | Built into .NET 6+ |

### Target Framework

```xml
<TargetFramework>net10.0-windows</TargetFramework>
<UseWindowsForms>true</UseWindowsForms>
```
