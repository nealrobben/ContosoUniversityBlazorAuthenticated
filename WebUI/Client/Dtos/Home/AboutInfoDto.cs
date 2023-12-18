using System.Collections.Generic;

namespace WebUI.Client.Dtos.Home;

public class AboutInfoDto
{
    public IEnumerable<EnrollmentDateGroupDto> Items { get; set; }

    public AboutInfoDto()
    {
        Items = [];
    }

    public AboutInfoDto(IEnumerable<EnrollmentDateGroupDto> items)
    {
        Items = items;
    }
}
