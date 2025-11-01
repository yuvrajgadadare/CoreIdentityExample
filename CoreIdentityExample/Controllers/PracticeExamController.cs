using ERPSystem_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class PracticeExamController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        EmailSettings _settings;
        IExamService examService;
        ITopicService topicService;
        public PracticeExamController(IMasterService masterService, IStudentService studentService, IOptions<EmailSettings> settings, IExamService examService, ITopicService topicService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this._settings = settings.Value;
            this.examService = examService;
            this.topicService = topicService;
        }
        public async Task< IActionResult> GetPracticeExams()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");
            }
           
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<ExamModel>exams=await examService.GetStudentWisePracticeExams(student_id);
            return View(exams);
        }

        public async Task<IActionResult> ViewPracticeExam(int id)
        {
          ExamModel e=await Task.Run(()=> examService.GetPracticeExam(id));
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<ExamModel> exams =await examService.GetStudentWisePracticeExams(student_id);
            ViewData["exams"] = exams;
            return View(e);
        }
        public async Task< IActionResult> Index(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");
            }
            TopicModel topic=await Task.Run(()=> topicService.GetTrainingTopic(id));
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            ExamModel exam = new ExamModel() 
            {
              student_id = student_id,
               exam_date = DateTime.Now,
                topic_id = id ,
                 topic_name=topic.topic_name,
                  total_questions=50,
                   status="Practice Exam"

            };
            List<ContentQuestionModel> questions =await topicService.GetTopicWiseQuestions(id, exam.total_questions);

            HttpContext.Session.SetString("practiceexam", JsonConvert.SerializeObject(exam));
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questions));
            return View(exam);
        }
        public async Task<IActionResult> StartExam(int? question_id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");
            }
            string exam = HttpContext.Session.GetString("practiceexam");
            ExamModel ex = (ExamModel)JsonConvert.DeserializeObject<ExamModel>(exam);
            ex.start_time = DateTime.Now;
            HttpContext.Session.SetString("practiceexam", JsonConvert.SerializeObject(ex));


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
                return Redirect("/Student/Login");
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
                string exatdata = HttpContext.Session.GetString("practiceexam");
                ExamModel exam = (ExamModel)JsonConvert.DeserializeObject<ExamModel>(exatdata);
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
                examService.SubmitPracticeExam(exam);
                return RedirectToAction("SubmitExam");
            }
            ModelState.Clear();
            return View(q);
        }

        public async Task<IActionResult> SubmitExam()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return Redirect("/Student/Login");
            }
            ViewBag.msg = "Exam Submitted Successfully.Please check your mail for exam result.";
            return View();
        }
    }
}
