using System;
using System.Collections.Generic;

namespace App_EmployeeManagement.DBModel;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? DepartmentId { get; set; }

    public virtual EmployeeDept? Department { get; set; }
}
