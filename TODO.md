# WeatherApp — TODO & Roadmap

Items are grouped by priority. PRs welcome — see [CONTRIBUTING.md](CONTRIBUTING.md).

---

## High Priority

### 5-Day Forecast
- [ ] Add a forecast strip below the current conditions panel
- [ ] Call OpenWeatherMap `/forecast` endpoint (returns 3-hour blocks for 5 days)
- [ ] Group by day and show high / low / condition icon for each
- [ ] Add `ForecastResponse` and `ForecastItem` models
- [ ] Add `GetForecastAsync()` to `WeatherService`

### Temperature Unit Toggle (°F / °C)
- [ ] Add a toggle button or checkbox in the footer (e.g., `°F | °C`)
- [ ] Store the selected unit in user settings (persisted across sessions)
- [ ] Update `appsettings.json` to reflect the choice, or store separately in `%AppData%`
- [ ] Switch `units` parameter between `imperial` and `metric` on toggle
- [ ] Re-fetch and re-display data immediately on toggle

### Weather Alerts
- [ ] Call OpenWeatherMap One Call API to retrieve active weather alerts
- [ ] Display an alert banner at the top of the form when alerts are active
- [ ] Color-code alerts by severity (yellow / orange / red)
- [ ] Allow dismissing the alert banner

---

## Medium Priority

### Multiple Cities
- [ ] Add a city management panel or dropdown
- [ ] Allow adding / removing cities in `appsettings.json` or a separate `cities.json`
- [ ] Show a tab or sidebar to switch between saved cities
- [ ] Remember the last selected city between sessions

### Auto-Refresh
- [ ] Add a configurable auto-refresh interval (e.g., every 10 / 30 / 60 minutes)
- [ ] Show a countdown to the next refresh in the footer
- [ ] Configurable via `appsettings.json` (`"AutoRefreshMinutes": 30`)

### System Tray Support
- [ ] Minimize to system tray instead of closing
- [ ] Show current temperature in the tray icon tooltip
- [ ] Right-click tray menu: Open, Refresh, Exit

---

## Low Priority

### Hourly Forecast
- [ ] Show a scrollable hourly strip for the next 12–24 hours
- [ ] Display time, icon, and temperature for each hour

### UV Index & Air Quality
- [ ] Add UV index from the One Call API
- [ ] Add Air Quality Index (AQI) from the Air Pollution API
- [ ] Color-code AQI (good / moderate / unhealthy)

### Sunrise / Sunset
- [ ] Display sunrise and sunset times from the current weather response (`sys.sunrise`, `sys.sunset`)
- [ ] Show a day/night progress indicator

### Settings Window
- [ ] Dedicated settings form for API key, city, units, auto-refresh interval
- [ ] Remove the need to manually edit `appsettings.json`
- [ ] Persist settings to `%AppData%\WeatherApp\settings.json`

### Installer / Packaging
- [ ] Publish as a self-contained `.exe` with `dotnet publish`
- [ ] Create a WiX or NSIS installer
- [ ] Add to Windows startup on user request

---

## Completed

- [x] Fetch current weather from OpenWeatherMap API
- [x] Display temperature, feels like, conditions, humidity, wind speed
- [x] Windows Forms window with dark modern theme
- [x] Weather-condition-driven background color
- [x] Animated loading spinner
- [x] Refresh button
- [x] Inline error display
- [x] `appsettings.json` configuration
