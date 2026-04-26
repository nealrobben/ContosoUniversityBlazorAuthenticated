namespace WebUINew.Client.Dtos.Departments;

public class DepartmentDetailDto
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }

    public int? InstructorId { get; set; }

    public byte[] RowVersion { get; set; }
}