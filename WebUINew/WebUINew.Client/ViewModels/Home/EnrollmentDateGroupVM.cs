using System.ComponentModel.DataAnnotations;

namespace WebUINew.Client.ViewModels.Home;

public class EnrollmentDateGroupVM
{
    [DataType(DataType.Date)]
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}