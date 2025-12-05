using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

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
        IEmployeeService employeeService;
        public GuestController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService, IEmployeeService employeeService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
            this.employeeService = employeeService;
        }
        public async Task< IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            List<StudentModel> lst = await studentService.GetGuestStudents(d.branch_id);
            return View(lst);
        }
    }
}
