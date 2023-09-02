namespace WebUI.Client.Components;

using Microsoft.AspNetCore.Components;

public partial class ProfilePicture
{
    private string _fileName;
    private const string emptyUrl = "img/person.png";

    [Parameter]
    public string FileName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_fileName))
            {
                return string.Concat("api/File/", _fileName);
            }
            else
            {
                return emptyUrl;
            }
        }
        set
        {
            _fileName = value;
        }
    }
}
