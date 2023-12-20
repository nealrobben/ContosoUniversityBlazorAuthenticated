using System.Collections.Generic;

namespace WebUI.Client.ViewModels.Instructors;

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
