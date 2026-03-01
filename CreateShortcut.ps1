# CreateShortcut.ps1
# Creates a "Weather App" desktop shortcut pointing to the Release build.
# Run from the solution root: .\CreateShortcut.ps1

$exePath  = Join-Path $PSScriptRoot "WeatherApp\bin\Release\net10.0-windows\WeatherApp.exe"
$icoPath  = Join-Path $PSScriptRoot "WeatherApp\WeatherApp.ico"
$lnkPath  = Join-Path ([Environment]::GetFolderPath('Desktop')) "Weather App.lnk"

if (-not (Test-Path $exePath)) {
    Write-Error "Release EXE not found at: $exePath"
    Write-Error "Run: dotnet build WeatherApp/WeatherApp.csproj --configuration Release"
    exit 1
}

$shell            = New-Object -ComObject WScript.Shell
$shortcut         = $shell.CreateShortcut($lnkPath)
$shortcut.TargetPath       = $exePath
$shortcut.WorkingDirectory = Split-Path $exePath
$shortcut.IconLocation     = "$icoPath,0"
$shortcut.Description      = "Current Weather for Grand Prairie TX"
$shortcut.Save()

Write-Host "Shortcut created: $lnkPath" -ForegroundColor Green
