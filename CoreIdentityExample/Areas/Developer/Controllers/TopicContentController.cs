using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class TopicContentController : Controller
    {
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        ICourseService courseService;
        public TopicContentController(IMasterService masterService, ITopicService topicService, IContentService contentService, ICourseService courseService)
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

        public async Task<JsonResult> GetContents()
        {
            List<ContentModel> lst = await contentService.GetAllTopicContents();
            return Json(lst);
        }
        public async Task<JsonResult> GetTopicWiseContents(int topic_id)
        {
            List<ContentModel> lst = await contentService.GetTopicWiseContents(topic_id);
            return Json(lst);
        }
        //public IActionResult AddContent()
        //{
        //    ViewBag.topics = new SelectList(masterService.GetTrainingTopics(), "topic_id", "topic_name");

        //    return View();
        //}
        [HttpPost]
        public async Task<string> AddContents(TopicModel t)
        {
         await contentService.AddTopicContent(t);

            return "Contents Added Successfully";
        }
        [HttpPost]
        public async Task<string> DeleteContent(int content_id)
        {
          await contentService.DeleteTopicContent(content_id);
            return "Content Deleted Successfully";
        }
        //[HttpGet]
        //public JsonResult GetTopicContents(int id)
        //{
        //    return Json(GetTopicWiseContents(id));
        //}
    }
}
