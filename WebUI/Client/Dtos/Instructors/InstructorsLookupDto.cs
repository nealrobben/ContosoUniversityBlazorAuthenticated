using System.Collections.Generic;

namespace WebUI.Client.Dtos.Instructors;

public record InstructorsLookupDto(IList<InstructorLookupDto> Instructors)
{
    public InstructorsLookupDto() : this([])
    {
    }
}