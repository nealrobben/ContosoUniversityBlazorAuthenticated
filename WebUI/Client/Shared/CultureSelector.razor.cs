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

    private CultureInfo[] supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("nl-NL"),
    };

    private CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
    }

    private async Task ChangeLanguageAsync(CultureInfo culture)
    {
        if (CultureInfo.CurrentCulture != culture)
        {
            await ClientPreferenceService.SetCulture(culture.Name);
            Nav.NavigateTo(Nav.Uri, forceLoad: true);
        }
    }
}
