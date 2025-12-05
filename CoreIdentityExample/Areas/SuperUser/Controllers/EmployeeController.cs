using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreIdentityExample.Areas.SuperUser.Controllers
{
    [Area("SuperUser")]
    public class EmployeeController : Controller
    {
        IEmployeeService employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
       
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);
            List<EmployeeModel> lst =await employeeService.GetEmployees(emp.branch_id);
            return View(lst);
        }
    }
}
