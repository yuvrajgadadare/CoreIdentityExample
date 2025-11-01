using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Trainer.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
