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
            //ViewData["qualifications"] = await courseService.GetTrainingCourses();
            StudentModel sm = new StudentModel() { };
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
