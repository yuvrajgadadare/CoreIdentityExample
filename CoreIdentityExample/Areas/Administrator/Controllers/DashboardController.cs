using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Administrator.Controllers
{
    [Area(areaName: "Administrator")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
