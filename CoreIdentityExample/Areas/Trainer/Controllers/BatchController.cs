using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CoreIdentityExample.Areas.Trainer.Controllers
{
       [Area("Trainer")]
    [Authorize]
        public class BatchController : Controller
        {
            IBatchService batchService;
            IMasterService masterService;
            private readonly UserManager<ApplicationUser> userManager;
            //signInManager will hold the SignInManager instance
            private readonly SignInManager<ApplicationUser> signInManager;
            //Both UserManager and SignInManager services are injected into the AccountController
            //using constructor injection
            IEmployeeService employeeService;
            IExtraService extraService;
     static int branch_id;
        public BatchController(IBatchService batchService, IMasterService masterService, IEmployeeService employeeService, IExtraService extraService, UserManager<ApplicationUser> userManager)
        {
            this.batchService = batchService;
            this.masterService = masterService;
            this.employeeService = employeeService;
            this.extraService = extraService;
            this.userManager = userManager;
          

        }
        public async Task<IActionResult> Index()
            {

            string uname = User.Identity.Name;
            ViewBag.user = uname;
            string userId = userManager.GetUserId(User);
            EmployeeModel employee = await employeeService.GetEmployeeByUserId(userId);
            branch_id = employee.branch_id;

            ViewData["trainer"] = employee;

                List<BatchModel> lst = await batchService.GetTrainerWiseBatches(employee.employee_id);
                return View(lst);
            }

            public async Task<IActionResult> Details(int id)
            {
                string uname = User.Identity.Name;
                ViewBag.user = uname;
                string userId = userManager.GetUserId(User);
                EmployeeModel employee = await employeeService.GetEmployeeByUserId(userId);

                ViewData["trainer"] = employee;

                BatchModel b = await batchService.GetBatch(id);
                    ViewBag.batch = b;

                List<BatchScheduleModel> schedule = await batchService.GetBatchWiseSchedule(id);

                List<BatchStudentModel> students = await batchService.GetBatchWiseStudents(id);
                ViewBag.schedule = schedule;
                ViewBag.students = students;
                return View();
            }

            public async Task<JsonResult> GetScheduleWiseData(int id)
            {
           

            BatchScheduleModel bs = await batchService.GetScheduleWiseSchedule(id);
                return Json(bs);
            }

            [HttpPost]
            public async Task<string> MarkAttendance(ScheduleAttendanceModel sm)
            {
                await batchService.MarkStudentScheduleAttendance(sm);

                return "Success";
            }

            public async Task<JsonResult> GetBatchWiseStudentAtendance(int batch_id, int registration_id)
            {
                List<StudentMarkAttendance> lst = await batchService.GetBatchWiseStudentAttendance(batch_id, registration_id);
                return Json(lst);
            }
        }
    }
 
