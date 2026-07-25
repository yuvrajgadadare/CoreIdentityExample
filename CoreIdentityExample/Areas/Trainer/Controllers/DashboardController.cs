using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Trainer.Controllers
{
    [Area("Trainer")]
    [Authorize]
    public class DashboardController : Controller
    {
     
        public IActionResult Index()
        {
            return View();
        }
    }
}
