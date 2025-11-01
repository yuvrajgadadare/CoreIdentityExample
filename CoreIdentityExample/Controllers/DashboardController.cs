using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreIdentityExample.Controllers
{
    public class DashboardController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IBatchService batchService;
        IExtraService extraService;
        private IWebHostEnvironment environment;
        EmailSettings _settings;
        IExamService examService;
        ICourseService courseService;
        ITopicService topicService;
        IContentService contentService;
        public DashboardController(IMasterService masterService, IStudentService studentService, IBatchService batchService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IExamService examService, ICourseService courseService, ITopicService topicService, IContentService contentService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.batchService = batchService;
            this.environment = environment;
            this.extraService = extraService;
            this._settings = settings.Value;
            this.examService = examService;
            this.courseService = courseService;
            this.topicService = topicService;
            this.contentService = contentService;
        }
        [Microsoft.AspNetCore.Mvc.Route("dashboard")]
        public async Task<IActionResult> Index()
        {
            int student_id =(int) HttpContext.Session.GetInt32("student_id");
            List<RegistrationModel> lst =await studentService.GetStudentWiseRegistrations(student_id);
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            List<StudentPaymentModel> payments =await studentService.GetRegistrationWisePayments(lst[0].registration_id);
            List<TopicModel> topics = await contentService.GetCourseWiseTopicAndContents(lst[0].course_id);
            ViewData["registration"] = lst[0];
            ViewData["batches"]= batches;
            ViewData["payments"]= payments;
            ViewData["topics"] = topics;
            ViewData["schedule"] =await studentService.GetStudentRegistrationWiseCourseSchedule(lst[0].registration_id);
            //List<ExamModel> exams = new List<ExamModel>();
            //foreach (BatchStudentModel b in batches)
            //{
            //    List<ExamModel> examlist = await examService.GetRegistrationAndBatchWiseScheduledExams(b.batch_id, lst[0].registration_id);
            //    exams.AddRange(examlist);
            //}
            ViewData["scheduledexams"] = await examService.GetStudentWiseScheduledBatchExams(lst[0].registration_id);
            return View();
        }
        public async Task<JsonResult> GetBatchWiseStudentAtendance(int batch_id, int registration_id)
        {
            List<StudentMarkAttendance> lst = await batchService.GetBatchWiseStudentAttendance(batch_id, registration_id);
            return Json(lst);
        }
    }
}
