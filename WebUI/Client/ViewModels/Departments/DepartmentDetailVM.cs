using System;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Client.ViewModels.Departments;

public class DepartmentDetailVM
{
    public int DepartmentID { get; set; }

    public string Name { get; set; }

    [DataType(DataType.Currency)]
    public decimal Budget { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime StartDate { get; set; }

    public string AdministratorName { get; set; }

    public int? InstructorID { get; set; }

    public byte[] RowVersion { get; set; }

    public override string ToString()
    {
        return $"{DepartmentID} - {Name}";
    }
}
