using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CoreIdentityExample.Controllers
{
    public class BatchExamController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        EmailSettings _settings;
        IExamService examService;
        ITopicService topicService;
        IContentService contentService;
        public BatchExamController(IMasterService masterService, IStudentService studentService, IOptions<EmailSettings> settings, IExamService examService, ITopicService topicService, IContentService contentService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this._settings = settings.Value;
            this.examService = examService;
            this.topicService = topicService;
            this.contentService = contentService;
        }
        public async Task<IActionResult> Index(int id)
        {
            int branch_id = (int)HttpContext.Session.GetInt32("branch_id");

            BatchExamModel exam = await examService.GetBatchExamByExamId(id);
            List<ExamModel> examlist = await examService.ViewAllScheduleExams(branch_id);
            if (exam == null)
            {
                ViewBag.msg = "Exam has expired. Please contact to the branch";
                return View();

            }
            string examtime = exam.exam_date.ToLongDateString() + " " + exam.start_time.ToLongTimeString();
            DateTime dt = Convert.ToDateTime(examtime);
            //if (dt.AddHours(1) <= DateTime.Now)
            //{
            //  //  examService.RejectScheduledExam(exam_id);
            //    ViewBag.status = false;
            //    ViewBag.msg = "Exam has expired. Please contact to the branch";
            //    return View();
            //}
            //else
            //{
            //    ViewBag.status = true;
            //}
            HttpContext.Session.SetString("exam", JsonConvert.SerializeObject(exam));
         //   TopicModel topic = await topicService.GetTrainingTopic(exam.topic_id);//.FirstOrDefault(e => e.topic_id.Equals(exam.topic_id));
            ViewBag.topic = exam.topic_name;
            ViewBag.question_count = exam.total_questions;
            List<ContentQuestionModel> questions = await topicService.GetTopicWiseQuestions(exam.topic_id, exam.total_questions);
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questions));
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            string exatdata = HttpContext.Session.GetString("exam");
            BatchExamModel exam = (BatchExamModel)JsonConvert.DeserializeObject<BatchExamModel>(exatdata);
            exam.start_time = DateTime.Now;
            HttpContext.Session.SetString("exam", JsonConvert.SerializeObject(exam));

            return RedirectToAction("StartExam");
        }

        public async Task<IActionResult> StartExam(int? question_id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;
            ContentQuestionModel q = null;
            if (question_id != null)
            {
                q = questionlist.FirstOrDefault(e => e.question_id.Equals(question_id));
                if (q.serial_number < questionlist.Count)
                {
                    ViewBag.next = false;
                }
                else
                {
                    ViewBag.next = true;

                }
                if (q.serial_number > 1)
                {
                    ViewBag.prev = false;
                }
                else
                {
                    ViewBag.prev = true;

                }
            }
            else
            {
                q = questionlist.First();
                ViewBag.prev = true;

            }
            //ContentQuestionModel q = questionlist.First();


            return View(q);
        }
        [HttpPost]
        public async Task<IActionResult> StartExam(ContentQuestionModel cm, string command)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            string questions = HttpContext.Session.GetString("questions");
            List<ContentQuestionModel> questionlist = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
            ViewBag.questions = questionlist;
            ContentQuestionModel qs = questionlist.FirstOrDefault(e => e.question_id.Equals(cm.question_id));
            int index = questionlist.IndexOf(qs);
            qs.submitted_option_number = cm.submitted_option_number;
            questionlist[index] = qs;
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questionlist));
            ContentQuestionModel q = null;
            if (command == null)
            {
                q = questionlist[0];
            }
            else if (command == "Next")
            {
                cm.serial_number++;
                if (cm.serial_number < questionlist.Count)
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.next = false;
                }
                else
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));

                    ViewBag.next = true;
                }
            }
            else if (command == "Prev")
            {
                cm.serial_number--;
                if (cm.serial_number > 1)
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.prev = false;
                }
                else
                {
                    q = questionlist.FirstOrDefault(e => e.serial_number.Equals(cm.serial_number));
                    ViewBag.prev = true;
                }
            }
            else if (command == "Submit")
            {
                string exatdata = HttpContext.Session.GetString("exam");
                BatchExamModel exam = (BatchExamModel)JsonConvert.DeserializeObject<BatchExamModel>(exatdata);
                exam.end_time = DateTime.Now;
                string questiondata = HttpContext.Session.GetString("questions");
                List<ContentQuestionModel> questionlistdata = (List<ContentQuestionModel>)JsonConvert.DeserializeObject<List<ContentQuestionModel>>(questions);
                List<ExamQuestionModel> lst = new List<ExamQuestionModel>();
                foreach (ContentQuestionModel c in questionlistdata)
                {
                    ExamQuestionModel e = new ExamQuestionModel()
                    {
                        question_id = c.question_id,
                        submitted_option_number = c.submitted_option_number
                    };
                    lst.Add(e);
                }
                exam.examQuestions = lst;
               await examService.SubmitBatchScheduledExam(exam);
                return RedirectToAction("SubmitExam");
            }
            ModelState.Clear();
            return View(q);
        }

        public async Task<IActionResult> SubmitExam()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
            return View();
        }

        //public async Task<IActionResult> GetPracticeExams()
        //{
        //    if (HttpContext.Session.GetInt32("student_id") == null)
        //    {
        //        return Redirect("/Student/Login");
        //    }

        //    int student_id = (int)HttpContext.Session.GetInt32("student_id");
        //    List<ExamModel> exams = await examService.GetStudentWisePracticeExams(student_id);
        //    return View(exams);
        //}

        public async Task<IActionResult> ViewBatchExam(int id)
        {
            BatchExamModel e = await  examService.GetBatchExam(id);
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            return View(e);
        }
    }
}
