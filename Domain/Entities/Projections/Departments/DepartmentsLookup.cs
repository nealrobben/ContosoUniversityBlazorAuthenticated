using System.Collections.Generic;

namespace Domain.Entities.Projections.Departments;

public class DepartmentsLookup
{
    public IList<DepartmentLookup> Departments { get; set; }
}
