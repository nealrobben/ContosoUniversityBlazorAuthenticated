namespace WebUINew.Client.ViewModels.Instructors;

public class InstructorLookupVM
{
    public int Id { get; set; }
    public string FullName { get; set; }

    public override string ToString()
    {
        return $"{Id} - {FullName}";
    }
}