using ERPSystem_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class ProgramController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IQuestionService questionService;
        ITopicService topicService;
        public ProgramController(IMasterService masterService, IStudentService studentService, IQuestionService questionService, ITopicService topicService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.questionService = questionService;
            this.topicService = topicService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            List<RegistrationModel> rlst = await studentService.GetStudentWiseRegistrations(student_id);// [0];
            RegistrationModel r =rlst[0];
            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            return View(topics);
        }
        public async Task<IActionResult> TopicWisePrograms(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            List<RegistrationModel> rlst = await studentService.GetStudentWiseRegistrations(student_id);// [0];
            RegistrationModel r = rlst[0];
            // RegistrationModel r = studentService.GetStudentWiseRegistrations(student_id)[0];
            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            ViewData["topics"] = topics;
            List<ProgramQuestionModel> programs =await questionService.GetTopicWiseProgramQuestions(id);
            return View(programs);
        }
    }
}
