
using System;

namespace Domain.Entities.Projections.Home;

public class EnrollmentDateGroup
{
    public DateTime? EnrollmentDate { get; set; }

    public int StudentCount { get; set; }
}
