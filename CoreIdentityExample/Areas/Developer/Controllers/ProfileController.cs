using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class ProfileController : Controller
    {
        IEmployeeService employeeService;
        public ProfileController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {

            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            string e = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(e);
            return View(emp);
        }
        [HttpPost]
        public async Task<IActionResult> Index(EmployeeModel em)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            EmployeeModel e =await employeeService.GetEmployee(em.employee_id);
            em.birth_date = em.birth_date;
            e.email_address = em.email_address;
            e.salary = em.salary;
           await employeeService.UpdateEmployeeDetails(e);
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ViewBag.msg = "Profile Details updated successfully";
            string es = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(es);
            return View(emp);
        }
    }
}
