using ERPSystem_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class BatchController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public BatchController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            return View(batches);
        }
    }
}
