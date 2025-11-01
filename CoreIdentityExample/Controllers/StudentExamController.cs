using Microsoft.AspNetCore.Mvc;
using ERPSystem_Models;
using ERP_Services.Interfaces;
using Newtonsoft.Json;
using ERP_Models;
namespace CoreIdentityExample.Controllers
{
    public class StudentExamController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IExtraService extraService;
        IExamService examService;
        public StudentExamController(IStudentService studentService, IMasterService masterService, IExtraService extraService, IExamService examService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.extraService = extraService;
            this.examService = examService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
             
            List<ExamModel> exams =await examService.GetStudentWiseExams(student_id);
            return View(exams);
          
        }

        public async Task< IActionResult> ViewExamDetails(int id)
        {
            if (HttpContext.Session.GetString("student_id") == null)
            {
                return Redirect("/Account/Login");
            }
          
            ExamModel em =await examService.GetExam(id);
            // int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
            return View(em);

        }
        public async Task<IActionResult> ExamCertificate(int id)
        {
            if (HttpContext.Session.GetString("student_id") == null)
            {
                return Redirect("/Account/Login");
            }
           
            ExamModel em =await examService.GetExam(id);
            return View(em);

        }
        public async Task<IActionResult>PrintCertificate(int id)
        {
            if (HttpContext.Session.GetString("student_id") == null)
            {
                return Redirect("/Account/Login");
            }
          
            ExamModel em =await examService.GetExam(id);
            StudentModel sm =await studentService.GetStudent(em.student_id);
            ViewData["student"] = sm;
            return View(em);

        }
    }
}
