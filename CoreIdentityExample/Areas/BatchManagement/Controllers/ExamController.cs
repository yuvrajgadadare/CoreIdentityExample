using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using ERPSystem_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;

namespace CIIT_ERPSystem.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
    [Authorize]

    public class ExamController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IExtraService extraService;
        EmailSettings _settings;
        IBatchService batchService;
        IContentService contentService;
        ITopicService topicService;
        IExamService examService;
        IEmployeeService employeeService;
        public static int currentPage = 0;

        public ExamController(IMasterService masterService, IStudentService studentService, IExtraService extraService, IOptions<EmailSettings> settings,IBatchService batchService, IContentService contentService, ITopicService topicService, IExamService examService,IEmployeeService employeeService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.extraService = extraService;
            this.batchService = batchService;
            _settings = settings.Value;
            this.contentService = contentService;
            this.topicService = topicService;
            this.examService = examService;
            this.employeeService = employeeService;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> ScheduleExam()
        {
            List<TopicModel> topics =await topicService.GetTrainingTopics();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;
            ViewBag.batches = new SelectList(await batchService.GetAllBatches(branch_id), "batch_id", "batch_name") ;
            List<StudentModel> studentlist = await GetAllStudents();
            IEnumerable<SelectListItem> items = studentlist.Select(e => new SelectListItem
            {
                 Value=e.student_id.ToString(),
                  Text=e.student_name+" "+e.last_name+"("+e.email_address+")"
            });
            ViewBag.students = new SelectList(items, "Value", "Text");
            ViewBag.topics=new SelectList(topics,"topic_id","topic_name");
            //int branch_id = (int)HttpContext.Session.GetInt32("branch_id");
            List<ExamModel> examlist =await examService.ViewAllScheduleExams(branch_id);
            foreach (ExamModel ex in examlist)
            {
                string examtime = ex.exam_date.ToLongDateString() + " " + ex.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt<= DateTime.Now)
                {
                  await examService.RejectScheduledExam(ex.exam_id);
                }
            }
           // int branch_id = (int)HttpContext.Session.GetInt32("branch_id");

            examlist = examService.ViewAllScheduleExams(branch_id).Result;
            ViewData["scheduledExams"] = examlist;
            ViewData["rejectedExams"] = examService.ViewAllRejectedExams(branch_id).Result;
            ViewData["submittedExams"] = examService.ViewAllSubmittedExams(branch_id).Result;
            ExamModel em = new ExamModel();
            return View(em);
        }
        public async Task<List<StudentModel>> GetAllStudents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;
            List<StudentModel> studentlist = await studentService.GetAllStudents(branch_id);
            return studentlist;
        }
        [HttpPost]
        public async Task<IActionResult> ScheduleExam(ExamModel em)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;

            int exam_id =await examService.ScheduleExamForStudent(em);
            string msg=await ShareExamLink(exam_id, em.student_id);
            ViewBag.msg= "Exam Scheduled Successfully," + msg;
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            List<TopicModel> topics =await topicService.GetTrainingTopics();
            //List<StudentModel> students = studentService.GetStudents();
            ViewBag.batches = new SelectList(await batchService.GetAllBatches(branch_id), "batch_id", "batch_name");
            //ViewData["students"] = await studentService.GetAllStudents();
            List<StudentModel> studentlist = await studentService.GetAllStudents(branch_id);
            IEnumerable<SelectListItem> items = studentlist.Select(e => new SelectListItem
            {
                Value = e.student_id.ToString(),
                Text = e.student_name + " " + e.last_name + "(" + e.email_address + ")"
            });
            ViewBag.students = new SelectList(items, "Value", "Text");
            ViewBag.topics = new SelectList(topics, "topic_id", "topic_name");
            List<ExamModel> examlist = await examService.ViewAllScheduleExams(branch_id);
            foreach (ExamModel ex in examlist)
            {
                string examtime = ex.exam_date.ToLongDateString() + " " + ex.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt <= DateTime.Now)
                {
                    await examService.RejectScheduledExam(ex.exam_id);
                }
            }

            examlist = await examService.ViewAllScheduleExams(branch_id);
            ViewData["scheduledExams"] = examlist;
            ViewData["rejectedExams"] =await examService.ViewAllRejectedExams(branch_id);
            ViewData["submittedExams"] =await examService.ViewAllSubmittedExams(branch_id);
            ExamModel employeemodel = new ExamModel();
            return View(employeemodel);
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            //List<TopicModel> topics = masterService.GetTrainingTopics();
            ////List<StudentModel> students = studentService.GetStudents();
            //ViewBag.students = GetStudents();
            //ViewData["scheduledExams"] = masterService.ViewAllScheduleExams();
            //ViewData["rejectedExams"] = masterService.ViewAllRejectedExams();
            //ViewData["submittedExams"] = masterService.ViewAllSubmittedExams();
            //ViewBag.topics = new SelectList(topics, "topic_id", "topic_name");
            //return View();
        }
        public async Task<List<SelectListItem>> GetStudents()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;

            List<StudentModel> students =await studentService.GetAllStudents(branch_id);
            foreach (var item in students)
            {
                lst.Add(new SelectListItem() {
                 Value=item.student_id.ToString(),
                  Text=item.student_name+" "+item.last_name
                });
            }
            return lst;
        }
        public async Task<string> ShareExamLink(int exam_id, int student_id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;

            try
            {
                 string examlink =DomainUrl.Url+ "/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;
                //  string examlink = "https://ciitstudent.com/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;

                StudentModel student =await studentService.GetStudent(student_id);

                List<ExamModel> exams =await examService.ViewAllScheduleExams(branch_id);
                    ExamModel exam=exams.FirstOrDefault(e => e.exam_id.Equals(exam_id));

                string examtime = exam.exam_date.ToLongDateString() + " " + exam.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt <= DateTime.Now)
                {
                    examService.RejectScheduledExam(exam_id);
                    return "Exam has been expired. cannot share link to the student";
                }
                else
                {
                    string message = "<h2>Dear " + student.student_name + ",</h2><p>Your exam has been scheduled for <b>" + exam.topic_name + "</b> on <b>" + exam.exam_date.ToLongDateString() + "</b> at Start Time:<b> " + exam.start_time.ToLongTimeString() + " </b>, End Time:<b>"+exam.end_time.ToLongTimeString()+"</b> </p><p><h4>Below is the exam link</h4></p><p><a target='_blank' href='" + examlink + "'>Click Here</a></p><br/><br/><h2>Best of Luck</h2><br/><p><h5>Regards,CIIT Training Institute Pvt Ltd</h5></p>";
                    EmailModel email = new EmailModel()
                    {
                        UserName = student.student_name,
                        EmailAddress = student.email_address,
                        Subject = "Exam Scheduled for " + exam.topic_name,
                        Message = message
                    };
                    extraService.SendEmail(email, _settings);
                    return "Exam Link Shared Successfully to the student";
                }
            }
            catch (Exception ex)
            {
                return "Unable to send exam link.send link again";
            }
        }
        public async  Task<IActionResult> Exams(int page = 0)
        {
            if (page == -1)
            {
                page = currentPage - 1;
            }
            if (page == -2)
            {
                page = currentPage + 1;
            }
            if (page == 0)
            {
                page = 1;
                ViewBag.prev = true;
            }
            else
            {
                ViewBag.prev = false;

            }
            currentPage = page;
            ViewBag.currentpage = page;
            int pageSize = 20;
            int totalPage = 0;
            int totalRecord = 0;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel d = await employeeService.GetEmployeeByUserId(userId);
            int branch_id = d.branch_id;
            List<ExamModel> lst =  GetExamData(page, pageSize, out totalRecord, out totalPage,branch_id);
            ViewBag.dbCount = totalPage;
            return View(lst);
        }
        public  List<ExamModel>  GetExamData(int page, int pageSize, out int totalRecord, out int totalPage,int branch_id)
        {
            List<ExamModel> query = new List<ExamModel>();
            List < ExamModel > data = examService.GetAllExams(branch_id).Result;
            totalRecord = data.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            query = data.OrderBy(a => a.exam_id).Skip(((page - 1) * pageSize)).Take(pageSize).ToList();
            return query;
        }
        public async Task< IActionResult> ViewExamDetails(int id)
        {
            ExamModel em =await examService.GetExam(id);
            return View(em);
        }
        public async Task<IActionResult> ExamCertificate(int id)
        {
            ExamModel em =await examService.GetExam(id);
            return View(em);
        }
        public async Task<IActionResult> PrintCertificate(int id)
        {
            ExamModel em =await examService.GetExam(id);
            StudentModel sm =await studentService.GetStudent(em.student_id);
            ViewData["student"] = sm;
            return View(em);
        }
    }
}