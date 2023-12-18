using System.Collections.Generic;

namespace Domain.Entities.Projections.Home;

public class AboutInfo
{
    public List<EnrollmentDateGroup> Items { get; set; }

    public AboutInfo()
    {
        Items = [];
    }

    public AboutInfo(List<EnrollmentDateGroup> items)
    {
        Items = items;
    }
}
