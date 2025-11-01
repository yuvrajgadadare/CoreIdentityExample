using Microsoft.AspNetCore.Mvc;
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class InterviewController : Controller
    {
        IQuestionService questionService;
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        ICourseService courseService;
        public InterviewController(IQuestionService questionService, IMasterService masterService, ITopicService topicService, IContentService contentService, ICourseService courseService)
        {
            this.questionService = questionService;
            this.masterService = masterService;
            this.topicService = topicService;
            this.contentService = contentService;
            this.courseService = courseService;
        }
        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //InterviewQuestionModel im = new InterviewQuestionModel();
            return View();
        }
        public async Task<IActionResult> AddInterviewQuestion()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");

            InterviewQuestionModel im = new InterviewQuestionModel();
            return View(im);
        }
        [HttpPost]
        public async Task<string> AddQuestions(InterviewQuestionFormModel q)
        {
          await   questionService.AddInterviewQuestions(q.content_id, q.questions);
            return "Added Successfully";
        }

        public async Task<JsonResult> GetQuestions()
        {
            List<InterviewQuestionModel> lst =await questionService.GetAllInterviewQuestions();
            return Json(lst);
        }
        public async Task<JsonResult> GetTopicWiseQuestions(int id)
        {
            List<InterviewQuestionModel> lst =await questionService.GetTopicWiseInterviewQuestions(id);
            return Json(lst);
        }
        public async Task<JsonResult> GetContentWiseQuestions(int id)
        {
            List<InterviewQuestionModel> lst =await questionService.GetContentWiseInterviewQuestions(id);
            return Json(lst);
        }
    }
}
