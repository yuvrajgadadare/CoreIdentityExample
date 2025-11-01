using Microsoft.AspNetCore.Mvc;
using ERP_Models;
using ERP_Services.Interfaces;
using Newtonsoft.Json;
namespace CoreIdentityExample.Controllers
{
    public class LearningController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IQuestionService questionService;
        ICourseService courseService;
        ITopicService topicService;
        IContentService contentService;
        public LearningController(IMasterService masterService, IStudentService studentService,IQuestionService questionService, ICourseService courseService, ITopicService topicService, IContentService contentService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.questionService = questionService;
            this.courseService = courseService;
            this.topicService = topicService;
            this.contentService = contentService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            //int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //StudentModel student =await studentService.GetStudent(student_id);
            //List<RegistrationModel> rst =await studentService.GetStudentWiseRegistrations(student_id) ;

            string reg = HttpContext.Session.GetString("registration");
            RegistrationModel r =(RegistrationModel)JsonConvert.DeserializeObject<RegistrationModel>(reg);


            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            return View(topics);
        }
        public async Task<IActionResult> TopicWiseQuestions(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            List<RegistrationModel> rst = await studentService.GetStudentWiseRegistrations(student_id);
            RegistrationModel r = rst[0];
            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            ViewData["topics"] = topics;    
            TopicModel tp =await topicService.GetTrainingTopic(id);
            ViewBag.topic = tp;
            List<InterviewQuestionModel> questions =await questionService.GetTopicWiseInterviewQuestions(id);
            List<ContentModel> contents =await contentService.GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(id);
            ViewData["contents"] = contents;

            return View(questions);
        }
        public async Task<IActionResult> ContentWiseInterviewQuestions(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            ContentModel content = await contentService.GetTopicContent(id);
            ViewData["content"] = content;
            List<RegistrationModel> rst = await studentService.GetStudentWiseRegistrations(student_id);
            RegistrationModel r = rst[0];
            List<TopicModel> topics = await topicService.GetCourseWiseTopics(r.course_id);
            ViewData["topics"] = topics;
            TopicModel tp = await topicService.GetTrainingTopic(content.topic_id);
            ViewBag.topic = tp;
            // List<ProgramQuestionModel> programs = questionService.GetTopicWiseProgramQuestions(id);
            List<InterviewQuestionModel> programs = await questionService.GetContentWiseInterviewQuestions(id);
            List<ContentModel> contents = await contentService.GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(tp.topic_id);


            ViewData["contents"] = contents;
            return View(programs);
        }

        public async Task< IActionResult> TopicWisePrograms(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student = await studentService.GetStudent(student_id);
            List<RegistrationModel> rst = await studentService.GetStudentWiseRegistrations(student_id);
            RegistrationModel r = rst[0];
            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            ViewData["topics"] = topics;
            TopicModel tp=await topicService.GetTrainingTopic(id);
            ViewBag.topic = tp;
            List<ProgramQuestionModel> programs =await questionService.GetTopicWiseProgramQuestions(id);
            List<ContentModel>contents=await contentService.GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(id);

            ViewData["contents"] = contents;
            return View(programs);
        }

        public async Task<IActionResult> ContentWisePrograms(int id)
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            StudentModel student =await studentService.GetStudent(student_id);
            ContentModel content =await contentService.GetTopicContent(id);
            ViewData["content"] = content;
            List<RegistrationModel> rst = await studentService.GetStudentWiseRegistrations(student_id);
            RegistrationModel r = rst[0];
            List<TopicModel> topics =await topicService.GetCourseWiseTopics(r.course_id);
            ViewData["topics"] = topics;
            TopicModel tp =await topicService.GetTrainingTopic(content.topic_id);
            ViewBag.topic = tp;
           // List<ProgramQuestionModel> programs = questionService.GetTopicWiseProgramQuestions(id);
            List<ProgramQuestionModel> programs =await questionService.GetContentWiseProgramQuestions(id);
            List<ContentModel> contents = await contentService.GetAllTopicWiseContentQuestionAndInterviewQuestionsCounts(tp.topic_id);


            ViewData["contents"] = contents;
            return View(programs);
        }
    }
}