using System.Collections.Generic;

namespace WebUI.Client.Dtos.Instructors;

public class InstructorsLookupDto
{
    public IList<InstructorLookupDto> Instructors { get; set; }

    public InstructorsLookupDto()
    {
        Instructors = [];
    }

    public InstructorsLookupDto(IList<InstructorLookupDto> instructors)
    {
        Instructors = instructors;
    }
}