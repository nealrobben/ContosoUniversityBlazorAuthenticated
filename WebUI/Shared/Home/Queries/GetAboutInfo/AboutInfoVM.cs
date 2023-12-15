namespace WebUI.Shared.Home.Queries.GetAboutInfo;

using System.Collections.Generic;

public class AboutInfoVM
{
    public List<EnrollmentDateGroup> Items { get; set; }

    public AboutInfoVM()
    {
        Items = [];
    }

    public AboutInfoVM(List<EnrollmentDateGroup> items)
    {
        Items = items;
    }
}
