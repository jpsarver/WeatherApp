using WeatherApp.Models;

namespace WeatherApp;

public class WeatherForm : Form
{
    // ── Controls ──────────────────────────────────────────────────────────
    private Label  _cityLabel        = null!;
    private Label  _lastUpdatedLabel = null!;
    private Label  _mainIconLabel    = null!;
    private Label  _tempLabel        = null!;
    private Label  _conditionsLabel  = null!;
    private Label  _feelsLikeLabel   = null!;
    private Label  _humidityLabel    = null!;
    private Label  _windLabel        = null!;
    private Panel  _detailsPanel     = null!;
    private Panel  _errorPanel       = null!;
    private Label  _errorLabel       = null!;
    private Panel  _headerPanel      = null!;
    private Panel  _footerPanel      = null!;
    private Button _refreshButton    = null!;
    private Panel  _loadingPanel     = null!;
    private Label  _loadingLabel     = null!;
    private System.Windows.Forms.Timer _dotTimer = null!;
    private int    _dotCount         = 0;

    public event EventHandler? RefreshRequested;

    // ── Layout constants ──────────────────────────────────────────────────
    private const int W            = 420;
    private const int HeaderH      = 82;
    private const int MainH        = 168;
    private const int DetailsH     = 156;
    private const int FooterH      = 72;
    private const int TotalH       = HeaderH + MainH + DetailsH + FooterH; // 478

    public WeatherForm()
    {
        SuspendLayout();
        InitializeComponents();
        ResumeLayout(false);
    }

    private void InitializeComponents()
    {
        Text            = "WeatherApp";
        ClientSize      = new Size(W, TotalH);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;
        StartPosition   = FormStartPosition.CenterScreen;
        BackColor       = Color.FromArgb(24, 90, 195);
        Font            = new Font("Segoe UI", 10f);

        BuildHeader();
        BuildMainSection();
        BuildDetailsSection();
        BuildFooter();
        BuildLoadingOverlay();

        _dotTimer = new System.Windows.Forms.Timer { Interval = 450 };
        _dotTimer.Tick += (_, _) =>
        {
            _dotCount = (_dotCount + 1) % 4;
            _loadingLabel.Text = "Fetching weather" + new string('.', _dotCount);
        };
    }

    // ── Header ─────────────────────────────────────────────────────────────
    private void BuildHeader()
    {
        _headerPanel = new Panel
        {
            Bounds    = new Rectangle(0, 0, W, HeaderH),
            BackColor = Color.FromArgb(14, 58, 138),
        };

        _cityLabel = new Label
        {
            Text      = "WeatherApp",
            Font      = new Font("Segoe UI", 20f, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(18, 10, W - 36, 46),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        _lastUpdatedLabel = new Label
        {
            Text      = "",
            Font      = new Font("Segoe UI", 8.5f),
            ForeColor = Color.FromArgb(155, 190, 235),
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(18, 58, W - 36, 18),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        _headerPanel.Controls.AddRange(new Control[] { _cityLabel, _lastUpdatedLabel });
        Controls.Add(_headerPanel);
    }

    // ── Main icon + temperature section ───────────────────────────────────
    private void BuildMainSection()
    {
        var mainPanel = new Panel
        {
            Bounds    = new Rectangle(0, HeaderH, W, MainH),
            BackColor = Color.Transparent,
        };

        // Large weather icon (emoji rendered via Segoe UI Emoji)
        _mainIconLabel = new Label
        {
            Text      = "☀️",
            Font      = new Font("Segoe UI Emoji", 58f),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(8, 8, 126, 126),
            TextAlign = ContentAlignment.MiddleCenter,
        };

        // Big temperature number
        _tempLabel = new Label
        {
            Text      = "--°F",
            Font      = new Font("Segoe UI", 44f, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(144, 14, 264, 96),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        // Conditions description below temperature
        _conditionsLabel = new Label
        {
            Text      = "",
            Font      = new Font("Segoe UI", 13f),
            ForeColor = Color.FromArgb(195, 225, 255),
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(148, 114, 260, 38),
            TextAlign = ContentAlignment.MiddleLeft,
        };

        mainPanel.Controls.AddRange(new Control[] { _mainIconLabel, _tempLabel, _conditionsLabel });
        Controls.Add(mainPanel);

        // Divider line
        Controls.Add(new Panel
        {
            Bounds    = new Rectangle(18, HeaderH + MainH, W - 36, 1),
            BackColor = Color.FromArgb(100, 160, 235),
        });
    }

    // ── Details rows (feels like / humidity / wind) ───────────────────────
    private void BuildDetailsSection()
    {
        int detailsY = HeaderH + MainH + 1;

        _detailsPanel = new Panel
        {
            Bounds    = new Rectangle(0, detailsY, W, DetailsH),
            BackColor = Color.FromArgb(14, 58, 138),
        };

        _feelsLikeLabel = DetailLabel(new Rectangle(20,  8, W - 40, 44));
        _humidityLabel  = DetailLabel(new Rectangle(20, 58, W - 40, 44));
        _windLabel      = DetailLabel(new Rectangle(20,108, W - 40, 44));

        _detailsPanel.Controls.AddRange(new Control[]
            { _feelsLikeLabel, _humidityLabel, _windLabel });

        // Error panel sits at same position, shown instead of detailsPanel
        _errorPanel = new Panel
        {
            Bounds    = new Rectangle(0, detailsY, W, DetailsH),
            BackColor = Color.FromArgb(70, 18, 18),
            Visible   = false,
        };

        _errorLabel = new Label
        {
            Text      = "",
            Font      = new Font("Segoe UI", 11f),
            ForeColor = Color.FromArgb(255, 130, 130),
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(16, 0, W - 32, DetailsH),
            TextAlign = ContentAlignment.MiddleCenter,
        };

        _errorPanel.Controls.Add(_errorLabel);
        Controls.AddRange(new Control[] { _detailsPanel, _errorPanel });
    }

    private static Label DetailLabel(Rectangle bounds) => new Label
    {
        Font      = new Font("Segoe UI", 12f),
        ForeColor = Color.FromArgb(210, 232, 255),
        BackColor = Color.Transparent,
        Bounds    = bounds,
        TextAlign = ContentAlignment.MiddleLeft,
    };

    // ── Footer with Refresh button ────────────────────────────────────────
    private void BuildFooter()
    {
        int footerY = HeaderH + MainH + DetailsH;

        _footerPanel = new Panel
        {
            Bounds    = new Rectangle(0, footerY, W, FooterH),
            BackColor = Color.FromArgb(14, 58, 138),
        };

        _refreshButton = new Button
        {
            Text      = "⟳   Refresh",
            Font      = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(38, 95, 205),
            FlatStyle = FlatStyle.Flat,
            Bounds    = new Rectangle(W / 2 - 95, 16, 190, 40),
            Cursor    = Cursors.Hand,
        };
        _refreshButton.FlatAppearance.BorderColor = Color.FromArgb(90, 140, 240);
        _refreshButton.FlatAppearance.BorderSize  = 1;
        _refreshButton.Click += (_, _) => RefreshRequested?.Invoke(this, EventArgs.Empty);

        _footerPanel.Controls.Add(_refreshButton);
        Controls.Add(_footerPanel);
    }

    // ── Loading overlay ───────────────────────────────────────────────────
    private void BuildLoadingOverlay()
    {
        // Covers the area between header and footer
        int overlayH = MainH + DetailsH + 1;

        _loadingPanel = new Panel
        {
            Bounds    = new Rectangle(0, HeaderH, W, overlayH),
            BackColor = Color.FromArgb(14, 58, 138),
            Visible   = false,
        };

        _loadingLabel = new Label
        {
            Text      = "Fetching weather",
            Font      = new Font("Segoe UI", 14f),
            ForeColor = Color.FromArgb(195, 225, 255),
            BackColor = Color.Transparent,
            Bounds    = new Rectangle(0, 0, W, overlayH),
            TextAlign = ContentAlignment.MiddleCenter,
        };

        _loadingPanel.Controls.Add(_loadingLabel);
        Controls.Add(_loadingPanel);
        _loadingPanel.BringToFront();
    }

    // ── Public API ────────────────────────────────────────────────────────
    public void ShowLoading()
    {
        if (InvokeRequired) { Invoke(ShowLoading); return; }
        _dotCount              = 0;
        _loadingLabel.Text     = "Fetching weather";
        _loadingPanel.Visible  = true;
        _refreshButton.Enabled = false;
        _errorPanel.Visible    = false;
        _dotTimer.Start();
        _loadingPanel.BringToFront();
    }

    public void HideLoading()
    {
        if (InvokeRequired) { Invoke(HideLoading); return; }
        _dotTimer.Stop();
        _loadingPanel.Visible  = false;
        _refreshButton.Enabled = true;
    }

    public void ShowWeather(WeatherResponse weather)
    {
        if (InvokeRequired) { Invoke(() => ShowWeather(weather)); return; }
        HideLoading();

        string city      = weather.CityName ?? "Unknown";
        double temp      = weather.Main?.Temp      ?? 0;
        double feelsLike = weather.Main?.FeelsLike ?? 0;
        int    humidity  = weather.Main?.Humidity  ?? 0;
        double wind      = weather.Wind?.Speed     ?? 0;
        string main      = weather.Weather?[0]?.Main        ?? "Clear";
        string desc      = weather.Weather?[0]?.Description ?? "";
        if (desc.Length > 0) desc = char.ToUpper(desc[0]) + desc[1..];

        _cityLabel.Text        = $"{city}, TX";
        _lastUpdatedLabel.Text = $"Updated {DateTime.Now:h:mm tt}";
        _tempLabel.Text        = $"{temp:F0}°F";
        _conditionsLabel.Text  = desc;
        _feelsLikeLabel.Text   = $"🌡️   Feels Like      {feelsLike:F1} °F";
        _humidityLabel.Text    = $"💧   Humidity          {humidity}%";
        _windLabel.Text        = $"💨   Wind Speed      {wind:F1} mph";

        _errorPanel.Visible   = false;
        _detailsPanel.Visible = true;

        ApplyTheme(main);
    }

    public void ShowError(string message)
    {
        if (InvokeRequired) { Invoke(() => ShowError(message)); return; }
        HideLoading();
        _detailsPanel.Visible = false;
        _errorPanel.Visible   = true;
        _errorLabel.Text      = message;
    }

    // ── Theme: icon + background colors driven by weather condition ───────
    private void ApplyTheme(string condition)
    {
        var (icon, formBg, darkBg) = condition.ToLowerInvariant() switch
        {
            "clear"        => ("☀️",  Color.FromArgb(24,  100, 215), Color.FromArgb(14,  68, 155)),
            "clouds"       => ("☁️",  Color.FromArgb(75,  88,  110), Color.FromArgb(52,  63,  82)),
            "rain"         => ("🌧️", Color.FromArgb(32,  52,   95), Color.FromArgb(22,  38,  72)),
            "drizzle"      => ("🌦️", Color.FromArgb(42,  64,  112), Color.FromArgb(30,  48,  88)),
            "thunderstorm" => ("⛈️", Color.FromArgb(22,  18,   55), Color.FromArgb(14,  12,  40)),
            "snow"         => ("❄️",  Color.FromArgb(148, 178, 215), Color.FromArgb(112, 142, 178)),
            "mist" or "smoke" or "haze" or "dust"
                or "fog"   or "ash" or "squall"
                or "tornado"
                           => ("🌫️", Color.FromArgb(82,  94,  112), Color.FromArgb(60,  70,  86)),
            _              => ("☀️",  Color.FromArgb(24,  90,  195), Color.FromArgb(14,  58,  138)),
        };

        _mainIconLabel.Text      = icon;
        BackColor                = formBg;
        _headerPanel.BackColor   = darkBg;
        _footerPanel.BackColor   = darkBg;
        _detailsPanel.BackColor  = darkBg;
        _loadingPanel.BackColor  = darkBg;
        _errorPanel.BackColor    = Color.FromArgb(70, 18, 18);
    }
}
