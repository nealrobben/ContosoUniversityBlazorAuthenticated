
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Threading.Tasks;
using WebUI.Client.Services;

namespace WebUI.Client.Shared;

public partial class CultureSelector
{
    [Inject]
    public NavigationManager Nav { get; set; }

    [Inject]
    public ClientSettingService ClientPreferenceService { get; set; }

    private readonly CultureInfo[] _supportedCultures =
    [
        new("en-US"),
        new("nl-NL"),
    ];

    private async Task ChangeLanguageAsync(CultureInfo culture)
    {
        if (CultureInfo.CurrentCulture != culture)
        {
            await ClientPreferenceService.SetCulture(culture.Name);
            Nav.NavigateTo(Nav.Uri, forceLoad: true);
        }
    }
}
