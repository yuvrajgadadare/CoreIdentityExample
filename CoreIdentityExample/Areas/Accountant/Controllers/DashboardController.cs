using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Accountant.Controllers
{
    [Area(areaName:"Accountant")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
