
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Threading.Tasks;
using WebUI.Client.Services;
using WebUI.Client.Settings;

namespace WebUI.Client.Shared;

public partial class MainLayout
{
    [Inject]
    public ClientSettingService ClientSettingService { get; set; }

    [Inject]
    public IStringLocalizer<MainLayout> Localizer { get; set; }

    private bool _drawerOpen = true;
    private MudTheme _currentTheme;

    protected override async Task OnInitializedAsync()
    {
        _currentTheme = await ClientSettingService.GetCurrentThemeAsync();
    }

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task ToggleDarkMode()
    {
        bool isDarkMode = await ClientSettingService.ToggleDarkModeAsync();
        _currentTheme = isDarkMode
            ? Themes.DefaultTheme : Themes.DarkTheme;
    }
}
