using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreIdentityExample.Areas.Accountant.Controllers
{
    [Area(areaName: "Accountant")]

    public class GuestController : Controller
    {

        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public GuestController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
        }
        public async Task< IActionResult> Index()
        {
            List<StudentModel> lst = await studentService.GetGuestStudents();
            return View(lst);
        }
    }
}
