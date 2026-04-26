namespace WebUINew.Client.Dtos.Departments;

public class UpdateDepartmentDto
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public byte[] RowVersion { get; set; }

    public int InstructorId { get; set; }
}