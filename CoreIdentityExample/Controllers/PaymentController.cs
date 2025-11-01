using ERPSystem_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class PaymentController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        public PaymentController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
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
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");  
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<BatchStudentModel> batches = batchService.GetStudentWiseBatches(student_id);

            List<StudentPaymentModel> lst =await studentService.GetStudentWisePayments(student_id);
            return View(lst);
        }

        public async Task<IActionResult> ViewInvoice(int registration_id, int payment_id)
        {
            if (HttpContext.Session.GetString("student_id") == null)
            {
                return Redirect("/Student/Login");


            }
            StudentPaymentModel p =await studentService.GetStudentPayment(payment_id);
            RegistrationModel r =await studentService.GetRegistration(registration_id);
            ViewData["registration"] = r;
            ViewData["payments"] = studentService.GetStudentWisePreviousPayments(registration_id, payment_id);
            ViewData["student"] = studentService.GetStudent(r.student_id);
            return View(p);
        }
    }
}
