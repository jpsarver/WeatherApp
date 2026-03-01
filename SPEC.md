# WeatherApp — Functional Specification

## 1. Purpose and Goals

WeatherApp provides at-a-glance current weather conditions for Grand Prairie, TX in a clean, always-on-top-friendly desktop window. The goal is a lightweight, zero-clutter experience: launch, see the weather, close.

### Goals

- Display real-time weather data with a single click
- Provide a visually intuitive experience where the UI reflects the weather
- Remain simple to configure and extend

---

## 2. Features and Requirements

### 2.1 Functional Requirements

| ID | Requirement |
|----|-------------|
| FR-01 | The app shall fetch current weather on startup |
| FR-02 | The app shall display temperature in °F (configurable to °C) |
| FR-03 | The app shall display "feels like" temperature |
| FR-04 | The app shall display weather conditions as a text description |
| FR-05 | The app shall display humidity as a percentage |
| FR-06 | The app shall display wind speed in mph (configurable to m/s) |
| FR-07 | The app shall show a loading animation while the API call is in progress |
| FR-08 | The app shall provide a Refresh button to re-fetch data without restarting |
| FR-09 | The app shall display errors inline without modal popups |
| FR-10 | The app shall read all configuration from `appsettings.json` |

### 2.2 Non-Functional Requirements

| ID | Requirement |
|----|-------------|
| NFR-01 | API response time should feel near-instant (< 2s on normal connections) |
| NFR-02 | The UI must remain responsive during API calls (async/await) |
| NFR-03 | The app shall not crash on network failure |
| NFR-04 | API keys shall not be hard-coded in source |

---

## 3. API Integration

### 3.1 Provider

**OpenWeatherMap** — Current Weather Data API
Endpoint: `https://api.openweathermap.org/data/2.5/weather`

### 3.2 Request

```
GET https://api.openweathermap.org/data/2.5/weather
  ?q={City},{State},{CountryCode}
  &appid={ApiKey}
  &units={Units}
```

| Parameter | Value |
|-----------|-------|
| `q` | `Grand Prairie,TX,US` |
| `appid` | API key from `appsettings.json` |
| `units` | `imperial` or `metric` |

### 3.3 Response Fields Used

```json
{
  "name": "Grand Prairie",
  "weather": [
    { "main": "Clear", "description": "clear sky" }
  ],
  "main": {
    "temp": 72.5,
    "feels_like": 70.1,
    "humidity": 45
  },
  "wind": {
    "speed": 9.3
  }
}
```

| Field | Displayed As |
|-------|-------------|
| `name` | Window title and city label |
| `weather[0].main` | Drives icon and color theme |
| `weather[0].description` | Conditions text (capitalized) |
| `main.temp` | Temperature |
| `main.feels_like` | Feels Like row |
| `main.humidity` | Humidity row |
| `wind.speed` | Wind Speed row |

### 3.4 Condition-to-Theme Mapping

| `weather.main` value | Icon | Background |
|----------------------|------|------------|
| `Clear` | ☀️ | Deep blue |
| `Clouds` | ☁️ | Slate grey |
| `Rain` | 🌧️ | Dark blue-grey |
| `Drizzle` | 🌦️ | Blue-grey |
| `Thunderstorm` | ⛈️ | Dark purple |
| `Snow` | ❄️ | Light steel blue |
| `Mist` / `Fog` / `Haze` / `Smoke` / etc. | 🌫️ | Cool grey |

---

## 4. UI/UX Specification

### 4.1 Window

| Property | Value |
|----------|-------|
| Size | 420 × 478 px (fixed) |
| Resizable | No |
| Maximizable | No |
| Start position | Center screen |

### 4.2 Sections

```
┌─────────────────────────────────────────┐  ← Header (82px)
│  Grand Prairie, TX                      │
│  Updated 3:45 PM                        │
├─────────────────────────────────────────┤  ← Main (168px)
│  ☀️          72°F                       │
│              Clear sky                  │
├─────────────────────────────────────────┤  ← Details (156px)
│  🌡️  Feels Like      70.1 °F           │
│  💧  Humidity          45%              │
│  💨  Wind Speed       9.3 mph           │
├─────────────────────────────────────────┤  ← Footer (72px)
│            [ ⟳  Refresh ]              │
└─────────────────────────────────────────┘
```

### 4.3 Loading State

A full-height animated overlay replaces the main + details sections while the API call is in progress. The label cycles through:

```
Fetching weather
Fetching weather.
Fetching weather..
Fetching weather...
```

The Refresh button is disabled during loading.

### 4.4 Error State

If the API call fails, the details panel is replaced by a red-tinted panel with the error message. The Refresh button remains enabled so the user can retry.

---

## 5. Error Handling Requirements

| Scenario | Behavior |
|----------|----------|
| Missing or empty API key | Show inline message: "API key not configured. Open appsettings.json and set your key." |
| Network unreachable | Show inline message: "Network error: \{message\}" |
| API returns non-2xx | `HttpRequestException` thrown by `WeatherService`, caught and displayed inline |
| Empty/null API response | `InvalidOperationException` thrown, displayed inline |
| Any unhandled exception | Caught by top-level handler, displayed inline |
| `appsettings.json` missing | `FileNotFoundException` on startup — app fails to start with .NET runtime error |
