using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentityExample.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
             
            return View();
        }
    }
}
