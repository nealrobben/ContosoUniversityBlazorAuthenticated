namespace WebUI.Client.Components;

using Microsoft.AspNetCore.Components;

public partial class ProfilePicture
{
    private string Link => !string.IsNullOrWhiteSpace(FileName) ? string.Concat("api/File/", FileName) : emptyUrl;
    private const string emptyUrl = "img/person.png";

    [Parameter]
    public string FileName { get; set; }
}
