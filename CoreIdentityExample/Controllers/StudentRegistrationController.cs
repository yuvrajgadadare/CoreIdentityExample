using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CoreIdentityExample.Controllers
{
    public class StudentRegistrationController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        ICourseService courseService;
        IEmployeeService employeeService;
        IBranchService branchService;
        public StudentRegistrationController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService, ICourseService courseService, IEmployeeService employeeService, IBranchService branchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
            this.courseService = courseService;
            this.employeeService = employeeService;
            this.branchService = branchService;
        }
        public async Task<List<SelectListItem>> GetQualifications()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (QualificationModel f in await masterService.GetQualifications())
            {
                SelectListItem sm = new SelectListItem()
                {
                    Text = f.qualification,
                    Value = f.qualification
                };
                lst.Add(sm);
            }
            return lst;
        }
        [Route("student-registration-form")]
        public async Task<IActionResult> StudentRegistrationForm()
        {
            ViewBag.courses = await GetFees();
            ViewBag.qualifications = await GetQualifications();
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            // ViewData["qualifications"] = await courseService.GetTrainingCourses();
            StudentModel sm = new StudentModel() { };
            return View(sm);
        }
        [Route("guest-registration-form")]
        public async Task<IActionResult> GuestRegistrationForm()
        {
            ViewBag.courses = new SelectList(await courseService.GetCoursesWithMinimumFees(),"fee_id","course_name");
            //  ViewBag.qualifications = await GetQualifications();
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            //ViewData["qualifications"] = await courseService.GetTrainingCourses();
            GuestRegistrationFormModel sm = new GuestRegistrationFormModel() { };
            return View(sm);
        }
        [HttpPost]
        [Route("guest-registration-form")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuestRegistrationForm(GuestRegistrationFormModel g)
        {
            if (ModelState.IsValid)
            {
                GuestRegistrationModel gr = new GuestRegistrationModel()
                {
                    birth_date = (DateTime)g.birth_date,
                    branch_id = (int)g.branch_id,
                    discount = 0,
                    email_address = g.email_address,
                    gender = g.gender,
                    mobile_number = g.mobile_number,
                    local_address = g.local_address,
                    last_name = g.last_name,
                    fee_id = (int)g.fee_id,
                    registration_date =(DateTime) g.registration_date,
                    student_name = g.student_name,
                    whatsapp_number = g.whatsapp_number
                };
                string pinnumber = await studentService.NextPINNumber();
                gr.permanent_identification_number = pinnumber;
                string password = await extraService.GetRandomPassword(10);
                gr.password = password;
                gr.student_code = "Student";
                string msg = await studentService.AddGuestStudentRegistration(gr);
                if (msg == "1")
                {
                    string message = "<h2>Dear<br/> " + g.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + g.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
                    EmailModel em = new EmailModel() { UserName = g.student_name, EmailAddress = g.email_address, Message = message, Subject = "Registration Confirmation" };
                    await extraService.SendEmail(em, _settings);
                    ViewBag.msg = "Your Account has been Created successfully.Please check your registered email.";

                }
                else
                {
                    if (msg.Contains("UNIQUE KEY"))
                    {
                        ViewBag.errormsg = "The given email address is already registered.please try another email address";

                    }
                }
            }
             
           

            ViewBag.courses = new SelectList(await courseService.GetCoursesWithMinimumFees(), "fee_id", "course_name");
            //  ViewBag.qualifications = await GetQualifications();
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            //ViewData["qualifications"] = await courseService.GetTrainingCourses();
            GuestRegistrationFormModel sm = new GuestRegistrationFormModel() { };
            return View(sm);
        }
        [HttpPost]
        public async Task<String> SubmitStudentRegistrationForm([FromBody] StudentModel sm)
        {
            string pinnumber = await studentService.NextPINNumber();
            sm.permanent_identification_number = pinnumber;
            string password = await extraService.GetRandomPassword(10);
            sm.password = password;
            sm.student_code = "Student";
            string msg = await studentService.AddStudentRegistration(sm);
            if (msg == "1")
            {
                string message = "<h2>Dear<br/> " + sm.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + sm.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
                EmailModel em = new EmailModel() { UserName = sm.student_name, EmailAddress = sm.email_address, Message = message, Subject = "Registration Confirmation" };
                await extraService.SendEmail(em, _settings);
                return "Student Registered Successfully";
            }
            else
            {
                return msg;
            }
        }
        public async Task<CourseFeeModel> GetCourseFee(int id)
        {
            CourseFeeModel f = await courseService.GetCourseFee(id);
            return f;
        }
        public async Task<List<SelectListItem>> GetFees()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (CourseFeeModel f in await courseService.GetCourseFees())
            {
                SelectListItem sm = new SelectListItem()
                {
                    Text = f.course_name + " (" + f.fee_mode + "-" + f.fees_amount + ")",
                    Value = f.fee_id.ToString()
                };
                lst.Add(sm);
            }
            return lst;
        }
        public async Task<List<SelectListItem>> GetCourses()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (CourseFeeModel c in await courseService.GetCourseFees())
            {
                SelectListItem s = new SelectListItem() { Text = c.course_name + "(" + c.fee_mode + "-" + c.fees_amount + ")", Value = c.fee_id.ToString() };
                lst.Add(s);
            }
            return lst;
        }
    }
}
