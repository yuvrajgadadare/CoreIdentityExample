using ERPSystem_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class VideoConferenceController : Controller
    {
        IStudentService studentService;
        public VideoConferenceController(IStudentService studentService)
        {
            this.studentService = studentService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> StartMeeting()
        {
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            ViewBag.name = student.student_name;
            return View();
        }
        public async Task<IActionResult> JoinMeeting()
        {
            return View();
        }
  
    }
}
