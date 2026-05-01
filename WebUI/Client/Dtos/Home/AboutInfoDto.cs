using System.Collections.Generic;

namespace WebUI.Client.Dtos.Home;

public record AboutInfoDto(IEnumerable<EnrollmentDateGroupDto> Items)
{
    public AboutInfoDto() : this([])
    {
    }
}