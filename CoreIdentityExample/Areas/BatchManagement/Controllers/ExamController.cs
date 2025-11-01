using ERP_Models;
using ERP_Services.Interfaces;
using ERPSystem_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CIIT_ERPSystem.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
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
        public static int currentPage = 0;

        public ExamController(IMasterService masterService, IStudentService studentService, IExtraService extraService, IOptions<EmailSettings> settings,IBatchService batchService, IContentService contentService, ITopicService topicService, IExamService examService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.extraService = extraService;
            this.batchService = batchService;
            _settings = settings.Value;
            this.contentService = contentService;
            this.topicService = topicService;
            this.examService = examService;
        }
        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            return View();
        }
        public async Task<IActionResult> ScheduleExam()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            List<TopicModel> topics =await topicService.GetTrainingTopics();
            //List<StudentModel> students = studentService.GetStudents();
            ViewBag.batches = new SelectList(await batchService.GetAllBatches(), "batch_id", "batch_name") ;
            //ViewBag.students = GetStudents();
            // ViewData["students"] = await studentService.GetAllStudents();

            List<StudentModel> studentlist = await GetAllStudents();
            IEnumerable<SelectListItem> items = studentlist.Select(e => new SelectListItem
            {
                 Value=e.student_id.ToString(),
                  Text=e.student_name+" "+e.last_name+"("+e.email_address+")"
            });
            ViewBag.students = new SelectList(items, "Value", "Text");
            ViewBag.topics=new SelectList(topics,"topic_id","topic_name");
            List<ExamModel> examlist =await examService.ViewAllScheduleExams();
            foreach (ExamModel ex in examlist)
            {
                string examtime = ex.exam_date.ToLongDateString() + " " + ex.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt<= DateTime.Now)
                {
                  await examService.RejectScheduledExam(ex.exam_id);
                }
            }
            examlist = examService.ViewAllScheduleExams().Result;
            ViewData["scheduledExams"] = examlist;
            ViewData["rejectedExams"] = examService.ViewAllRejectedExams().Result;
            ViewData["submittedExams"] = examService.ViewAllSubmittedExams().Result;
            ExamModel em = new ExamModel();
            return View(em);
        }

        public async Task<List<StudentModel>> GetAllStudents(){

            List<StudentModel> studentlist = await studentService.GetAllStudents();
            //foreach(var s in await studentService.GetAllStudents())
            //{
            //    studentlist.Add(new StudentModel { student_id = s.student_id, student_name = s.student_name + " " + s.last_name+"("+s.permanent_identification_number+")" });
               
            //}
            return studentlist;
        }

        //[HttpPost]
        //public string ScheduleExam(ExamModel em)
        //{
        //    int exam_id = masterService.ScheduleExamForStudent(em);
        //    string msg = ShareExamLink(exam_id, em.student_id);
        //    return "Exam Scheduled Successfully," + msg;
        //}
        [HttpPost]
        public async Task<IActionResult> ScheduleExam(ExamModel em)
        {
           int exam_id=await examService.ScheduleExamForStudent(em);
            string msg=await ShareExamLink(exam_id, em.student_id);
            ViewBag.msg= "Exam Scheduled Successfully," + msg;
            string employee = HttpContext.Session.GetString("employee");
            EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            ViewData["employee"] = emp;
            List<TopicModel> topics =await topicService.GetTrainingTopics();
            //List<StudentModel> students = studentService.GetStudents();
            ViewBag.batches = new SelectList(await batchService.GetAllBatches(), "batch_id", "batch_name");
            //ViewData["students"] = await studentService.GetAllStudents();
            List<StudentModel> studentlist = await studentService.GetAllStudents();
            IEnumerable<SelectListItem> items = studentlist.Select(e => new SelectListItem
            {
                Value = e.student_id.ToString(),
                Text = e.student_name + " " + e.last_name + "(" + e.email_address + ")"
            });
            ViewBag.students = new SelectList(items, "Value", "Text");
            ViewBag.topics = new SelectList(topics, "topic_id", "topic_name");
            List<ExamModel> examlist = await examService.ViewAllScheduleExams();
            foreach (ExamModel ex in examlist)
            {
                string examtime = ex.exam_date.ToLongDateString() + " " + ex.end_time.ToLongTimeString();
                DateTime dt = Convert.ToDateTime(examtime);
                if (dt <= DateTime.Now)
                {
                    await examService.RejectScheduledExam(ex.exam_id);
                }
            }
            examlist = await examService.ViewAllScheduleExams();
            ViewData["scheduledExams"] = examlist;
            ViewData["rejectedExams"] =await examService.ViewAllRejectedExams();
            ViewData["submittedExams"] =await examService.ViewAllSubmittedExams();
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
            List<StudentModel> students =await studentService.GetAllStudents();
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
            try
            {
                 string examlink =DomainUrl.Url+ "/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;
                //  string examlink = "https://ciitstudent.com/Assessment/Index?exam_id=" + exam_id + "&student_id=" + student_id;

                StudentModel student =await studentService.GetStudent(student_id);

                List<ExamModel> exams =await examService.ViewAllScheduleExams();
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
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            //int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<ExamModel> exams = masterService.GetStudentWiseExams(student_id);
          //  List<ExamModel> exams =await examService.GetAllExams();
          //  return View(exams);
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

            List<ExamModel> lst =  GetExamData(page, pageSize, out totalRecord, out totalPage);
            ViewBag.dbCount = totalPage;
            return View(lst);

        }


        public  List<ExamModel>  GetExamData(int page, int pageSize, out int totalRecord, out int totalPage)
        {
             
            List<ExamModel> query = new List<ExamModel>();
            List < ExamModel > data = examService.GetAllExams().Result;
            totalRecord = data.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            query = data.OrderBy(a => a.exam_id).Skip(((page - 1) * pageSize)).Take(pageSize).ToList();
            return query;
        }


        public async Task< IActionResult> ViewExamDetails(int id)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            ExamModel em =await examService.GetExam(id);
          //  int student_id = (int)HttpContext.Session.GetInt32("student_id");
          //  List<ExamModel> exams =await examService.GetStudentWiseExams(student_id);
            return View(em);

        }
        public async Task<IActionResult> ExamCertificate(int id)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            ExamModel em =await examService.GetExam(id);
            return View(em);

        }
        public async Task<IActionResult> PrintCertificate(int id)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            ExamModel em =await examService.GetExam(id);
            StudentModel sm =await studentService.GetStudent(em.student_id);
            ViewData["student"] = sm;
            return View(em);

        }
    }
}
