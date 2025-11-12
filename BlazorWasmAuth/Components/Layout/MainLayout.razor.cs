using MudBlazor;

namespace Frontend.Components.Layout;

public partial class MainLayout
{
    private bool _drawerOpen = true;
    private bool _settingsOpen = false;
    private bool _isDarkMode = false;
    private MudTheme? _theme = null;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _theme = new()
        {
            PaletteLight = _lightPalette,
            PaletteDark = _darkPalette,
            LayoutProperties = new LayoutProperties()
        };
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenSettingsToggle()
    {
        _settingsOpen = !_settingsOpen;
    }

    private void DarkModeToggle()
    {
        _isDarkMode = !_isDarkMode;
    }

    private readonly PaletteLight _lightPalette = new()
    {
        Black = "#110e2d",
        AppbarText = "#424242",
        AppbarBackground = "rgba(255,255,255,0.8)",
        DrawerBackground = "#ffffff",
        GrayLight = "#e8e8e8",
        GrayLighter = "#f9f9f9",
    };

    private readonly PaletteDark _darkPalette = new()
    {
        Primary = "#7e6fff",
        Surface = "#1e1e2d",
        Background = "#1a1a27",
        BackgroundGray = "#151521",
        AppbarText = "#92929f",
        AppbarBackground = "rgba(26,26,39,0.8)",
        DrawerBackground = "#1a1a27",
        ActionDefault = "#74718e",
        ActionDisabled = "#9999994d",
        ActionDisabledBackground = "#605f6d4d",
        TextPrimary = "#b2b0bf",
        TextSecondary = "#92929f",
        TextDisabled = "#ffffff33",
        DrawerIcon = "#92929f",
        DrawerText = "#92929f",
        GrayLight = "#2a2833",
        GrayLighter = "#1e1e2d",
        Info = "#4a86ff",
        Success = "#3dcb6c",
        Warning = "#ffb545",
        Error = "#ff3f5f",
        LinesDefault = "#33323e",
        TableLines = "#33323e",
        Divider = "#292838",
        OverlayLight = "#1e1e2d80",
    };


    //private readonly PaletteLight _lightPalette = new()
    //{
    //    Primary = "#FFB547",             // Warmes Sonnen-Orange (Hauptfarbe)
    //    Secondary = "#00A3B4",           // Frisches Türkis-Blau (Kontrastfarbe)
    //    Black = "#0D0C22",               // Tiefer Textkontrast
    //    AppbarText = "#2D2D2D",
    //    AppbarBackground = "rgba(255,255,255,0.9)",
    //    DrawerBackground = "#FFFFFF",
    //    Surface = "#FFFFFF",
    //    Background = "#F9FAFB",          // Sanftes Weiß-Grau
    //    GrayLight = "#E9ECEF",
    //    GrayLighter = "#F8F9FA",
    //    Info = "#3A86FF",
    //    Success = "#3DCB6C",
    //    Warning = "#FFB545",
    //    Error = "#FF5A5F",
    //    LinesDefault = "#E0E0E0",
    //    TextPrimary = "#222222",
    //    TextSecondary = "#555555",
    //};

    //private readonly PaletteDark _darkPalette = new()
    //{
    //    Primary = "#FFB547",             // Gleiche Hauptfarbe, strahlt auf Dunkel
    //    Secondary = "#00A3B4",           // Türkis-Akzent für Buttons/Links
    //    Surface = "#1E1E28",
    //    Background = "#14141C",
    //    BackgroundGray = "#1A1A24",
    //    AppbarText = "#B8B8C3",
    //    AppbarBackground = "rgba(20,20,28,0.9)",
    //    DrawerBackground = "#1A1A24",
    //    ActionDefault = "#9E9EAD",
    //    ActionDisabled = "#9999994D",
    //    ActionDisabledBackground = "#605F6D4D",
    //    TextPrimary = "#EAEAF2",
    //    TextSecondary = "#B8B8C3",
    //    TextDisabled = "#FFFFFF33",
    //    DrawerIcon = "#B8B8C3",
    //    DrawerText = "#B8B8C3",
    //    GrayLight = "#2A2833",
    //    GrayLighter = "#1E1E28",
    //    Info = "#4A86FF",
    //    Success = "#3DCB6C",
    //    Warning = "#FFB545",
    //    Error = "#FF3F5F",
    //    LinesDefault = "#2E2D39",
    //    TableLines = "#2E2D39",
    //    Divider = "#292838",
    //    OverlayLight = "#1E1E2D80",
    //};

    public string DarkLightModeButtonIcon => _isDarkMode switch
    {
        true => Icons.Material.Rounded.AutoMode,
        false => Icons.Material.Outlined.DarkMode,
    };
}