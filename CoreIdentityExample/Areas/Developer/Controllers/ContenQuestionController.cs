using Microsoft.AspNetCore.Mvc;
using ERP_Models;
using ERP_Services;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CIIT_ERPSystem.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class ContentQuestionController : Controller
    {
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        ICourseService courseService;
        public ContentQuestionController(IMasterService masterService, ITopicService topicService, IContentService contentService, ICourseService courseService)
        {
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
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            return View();
        }
        public async Task<IActionResult> AddQuestions()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");

            return View();
        }
        public async Task<JsonResult> GetTopicWiseContents(int id)
        {
            return Json(await contentService.GetTopicWiseContents(id));
        }
        public async Task<JsonResult> GetTopicWiseQuestions(int id)
        {
            return Json(await topicService.GetTopicWiseQuestions(id)); 
        }
        public async Task<JsonResult> GetContentWiseQuestions(int id)
        {
            return Json(await contentService.GetContentWiseQuestions(id));
        }
        public async Task<string> AddContentQuestions(ContentModel c)
        {
           await contentService.AddContentQuestion(c.content_id,c.contentQuestions);
            return "Questions Added Successfully";
        }

        public async Task<IActionResult> DeleteQuestion(int id)
        {
          await contentService.DeleteContentQuestion(id);
            return RedirectToAction("Index");
        }
    }
}
