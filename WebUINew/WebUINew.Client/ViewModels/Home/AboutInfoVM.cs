namespace WebUINew.Client.ViewModels.Home;

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