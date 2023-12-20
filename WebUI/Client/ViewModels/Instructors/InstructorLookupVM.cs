namespace WebUI.Client.ViewModels.Instructors;

public class InstructorLookupVM
{
    public int ID { get; set; }
    public string FullName { get; set; }

    public override string ToString()
    {
        return $"{ID} - {FullName}";
    }
}
