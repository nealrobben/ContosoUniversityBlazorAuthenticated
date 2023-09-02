namespace WebUI.Shared.Instructors.Queries.GetInstructorsLookup;

using System.Collections.Generic;

public class InstructorsLookupVM
{
    public IList<InstructorLookupVM> Instructors { get; set; }

    public InstructorsLookupVM()
    {
        Instructors = new List<InstructorLookupVM>();
    }

    public InstructorsLookupVM(IList<InstructorLookupVM> instructors)
    {
        Instructors = instructors;
    }
}
