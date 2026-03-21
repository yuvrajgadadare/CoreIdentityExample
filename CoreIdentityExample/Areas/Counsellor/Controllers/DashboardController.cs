using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Counsellor.Controllers
{
    [Area(areaName: "Counsellor")]

    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
