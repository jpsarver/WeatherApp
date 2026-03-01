# Contributing to WeatherApp

Thank you for your interest in contributing. This document covers the process for reporting issues, proposing changes, and submitting pull requests.

---

## Getting Started

1. **Fork** the repository on GitHub
2. **Clone** your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/WeatherApp.git
   cd WeatherApp
   ```
3. **Create a branch** for your work:
   ```bash
   git checkout -b feature/your-feature-name
   ```
4. **Configure** `appsettings.json` with your own OpenWeatherMap API key (never commit a real key to a public fork)

---

## Development Setup

**Requirements:**
- .NET SDK 10.0+
- Windows 10 / 11 (Windows Forms requires Windows)
- Any editor — Visual Studio 2022, VS Code with C# Dev Kit, or Rider

**Build and run:**
```bash
dotnet restore WeatherApp/WeatherApp.csproj
dotnet build  WeatherApp/WeatherApp.csproj
dotnet run   --project WeatherApp/WeatherApp.csproj
```

---

## Code Style

- Follow existing conventions — C# naming, `null!` for late-initialized fields, `async/await` for all I/O
- Keep UI construction in `WeatherForm.cs`; keep HTTP logic in `WeatherService.cs`
- All public `WeatherForm` methods must include `InvokeRequired` / `Invoke()` guards for thread safety
- No hard-coded API keys, cities, or credentials anywhere in source

---

## Submitting a Pull Request

1. Make sure the project **builds cleanly** (`0 warnings, 0 errors`)
2. Test manually — launch the app and verify your changes work end-to-end
3. Update `CHANGELOG.md` under a new `[Unreleased]` section describing what you added or changed
4. Update `TODO.md` to mark any completed items
5. Push your branch and open a PR against `master`:
   - Write a clear title and description
   - Reference any related issues with `Fixes #123`

---

## Reporting Issues

Open a GitHub Issue and include:
- What you expected to happen
- What actually happened
- Steps to reproduce
- .NET version (`dotnet --version`) and Windows version

---

## Roadmap

See [TODO.md](TODO.md) for planned features and improvements.
