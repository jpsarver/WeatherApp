# Changelog

All notable changes to WeatherApp are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

---

## [1.2.0] — 2026-02-28

### Added
- Full Windows Forms UI replacing the single-label display
- Weather-condition-driven background color theme (blue / grey / dark blue / purple / steel blue)
- Large emoji weather icon that changes with current conditions (☀️ ☁️ 🌧️ 🌦️ ⛈️ ❄️ 🌫️)
- Animated loading overlay with dot-spinner (`Fetching weather...`) while API call is in progress
- Refresh button to re-fetch live weather data without restarting
- Inline error display panel (red-tinted) replacing the details section on failure
- "Updated h:mm tt" timestamp in header

### Changed
- `Program.cs` refactored to use a shared `FetchWeatherAsync()` task wired to both `form.Load` and `form.RefreshRequested`
- All UI state transitions (`ShowLoading`, `HideLoading`, `ShowWeather`, `ShowError`) are thread-safe via `InvokeRequired`

---

## [1.1.0] — 2026-02-28

### Added
- `WeatherForm` with a dark-themed label replacing the `MessageBox` display

### Changed
- Replaced `MessageBox.Show()` with a persistent `Form` using `Application.Run()`
- `Program.cs` updated to use `ApplicationConfiguration.Initialize()` and `form.Load` async pattern

---

## [1.0.1] — 2026-02-28

### Fixed
- Retargeted from `net8.0-windows` to `net10.0-windows` — only .NET 10 is installed on the host machine

---

## [1.0.0] — 2026-02-28

### Added
- Initial project: .NET console app with `UseWindowsForms` for `MessageBox` display
- `WeatherService` calling OpenWeatherMap current weather API
- `WeatherResponse`, `WeatherCondition`, `MainData`, `WindData` JSON models
- `appsettings.json` for API key and city configuration (Grand Prairie, TX)
- Error handling for missing API key, network errors, and unexpected exceptions
- `.gitignore` for C# / .NET projects
- Git repository initialized and pushed to GitHub
