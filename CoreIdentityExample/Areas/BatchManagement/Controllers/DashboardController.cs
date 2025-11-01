using ERP_Models;
using ERPSystem_Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace CIIT_ERPSystem.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("employee") == null)
            {
                return Redirect("/Account/Login");
            }
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            return View();
        }
    }
}
