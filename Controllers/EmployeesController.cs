using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCrudApp.Data;
using SimpleCrudApp.Models;
using SimpleCrudApp.Models.Domain;

namespace SimpleCrudApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDBcontext mvcDemoDBcontext;

        public EmployeesController(MVCDemoDBcontext mVCDemoDBcontext)
        {
            this.mvcDemoDBcontext = mVCDemoDBcontext;
        }

        public MVCDemoDBcontext MvcDemoContext { get; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDBcontext.Employees.ToListAsync();
            return View(employees);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mvcDemoDBcontext.Employees.AddAsync(employee);
            await mvcDemoDBcontext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDBcontext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDBcontext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mvcDemoDBcontext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDBcontext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDBcontext.Employees.Remove(employee);
                await mvcDemoDBcontext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}
