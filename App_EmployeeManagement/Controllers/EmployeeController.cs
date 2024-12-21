using App_EmployeeManagement.DBModel;
using App_EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App_EmployeeManagement.Controllers
{
   
    public class EmployeeController : Controller
    {

        private readonly EmployeeManagementDbContext _context;

        public EmployeeController(EmployeeManagementDbContext context)
        {
            _context = context;
        }
        [Route("api/Employee")]
        public async Task<IActionResult> Employee()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();
            return View(employees); // Pass employees to the Index view
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, return a 404 error
            }

            // Fetch the employee from the database based on the provided ID
            var employee = await _context.Employees
                                         .Include(e => e.Department) // Optionally include related data like the Department
                                         .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(); // If the employee does not exist, return a 404 error
            }

            return View(employee); // Return the employee details to the view
        }

        public IActionResult CreateEmployee()
        {
            App_EmployeeManagement.DBModel.Employee employee = new DBModel.Employee();
            ViewBag.Departments = _context.EmployeeDepts.ToList(); // Fetch the departments from the database
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(App_EmployeeManagement.DBModel.Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Employee)); // Redirect to the list of employees after adding
            }
            // If the model is not valid, return the same view with the existing data
            ViewBag.Departments = _context.EmployeeDepts.ToList();
            return View(employee);
        }

       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync(); // Save changes to the database

            return RedirectToAction(nameof(Employee)); // Redirect to the employee list page after deletion
        }
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                                         .Include(e => e.Department) // Include department details if needed
                                         .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee); // Pass the employee object to the view for confirmation
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // If no ID is provided, return a 404 error
            }

            // Fetch the employee from the database by the provided ID
            var employee = await _context.Employees
                                         .Include(e => e.Department) // Optionally include related data like Department
                                         .FirstOrDefaultAsync(m => m.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(); // If the employee doesn't exist, return a 404 error
            }

            // Populate the departments (for dropdown or selection) in the view
            ViewBag.Departments = new SelectList(_context.EmployeeDepts, "DepartmentId", "DepartmentName", employee.DepartmentId);

            return View(employee); // Return the employee object to the view for editing
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Email,DepartmentId")] App_EmployeeManagement.DBModel.Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound(); // If the ID in the route doesn't match the employee ID, return a 404 error
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee); // Update the employee in the database
                    await _context.SaveChangesAsync(); // Save changes to the database
                    return RedirectToAction(nameof(Employee)); // Redirect to the employee list after the update
                }
                catch (Exception ex)
                {
                        throw ex; // Throw the exception if another error occurs
                }
            }
            // If the model state is invalid, return the view with the existing data
            ViewBag.Departments = new SelectList(_context.EmployeeDepts, "DepartmentId", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }


    }
}
