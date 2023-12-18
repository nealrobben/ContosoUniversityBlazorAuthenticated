
using System.Collections.Generic;

namespace WebUI.CLient.ViewModels.Home;

public class AboutInfoVM
{
    public List<EnrollmentDateGroupVM> Items { get; set; }

    public AboutInfoVM()
    {
        Items = [];
    }

    public AboutInfoVM(List<EnrollmentDateGroupVM> items)
    {
        Items = items;
    }
}
