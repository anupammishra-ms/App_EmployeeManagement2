using System;
using System.Collections.Generic;

namespace App_EmployeeManagement.DBModel;

public partial class EmployeeDept
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
