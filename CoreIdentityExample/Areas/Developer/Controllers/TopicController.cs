using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class TopicController : Controller
    {
        IExtraService extraService;
        IMasterService masterService;
        IWebHostEnvironment _environment;
        IContentService contentService;
        ITopicService topicService;
        ICourseService courseService;
        public TopicController(IExtraService extraService,IMasterService masterService, IWebHostEnvironment environment, IContentService contentService, ITopicService topicService, ICourseService courseService)
        {
            this.extraService = extraService;
            this.masterService = masterService;
            _environment = environment;
            this.contentService = contentService;
            this.topicService = topicService;
            this.courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            // ViewBag.topics = masterService.GetTrainingTopics();
            List<TopicModel> lst= await topicService.GetTrainingTopics();
            ViewData["topics"] = lst;
            return View();
        }
        [HttpPost]
        public IActionResult Index(TopicModel topic)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            topicService.AddTopic(topic);
            ViewBag.msg = "Topic Added Successfully";
            ModelState.Clear();
            ViewBag.topics = topicService.GetTrainingTopics();

            return View();
        }
        public async Task<JsonResult> GetTopics()
        {
            List<TopicModel> lst = await topicService.GetTrainingTopics();
            return Json(lst);
        }
        public async Task<IActionResult> Video()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            SelectList topics = new SelectList(await topicService.GetTrainingTopics(),"topic_id","topic_name");
            ViewBag.topics = topics;
            TopicVideoModel t = new TopicVideoModel();
            ViewBag.videos = await topicService.GetAllTopicVideos();
            return View(t);
        }
        [HttpPost]
        [RequestSizeLimit(1073741824)]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]

        //[RequestSizeLimit(2000L * 1024L * 1024L*1024L)]       //unit is bytes => 500Mb
        //[RequestFormLimits(MultipartBodyLengthLimit = 2000L * 1024L * 1024L * 1024L)]
        public async Task<IActionResult> Video(TopicVideoModel v, IFormFile video)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            string filename = v.topic_id + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + Path.GetExtension(video.FileName);
            string folderpath = _environment.WebRootPath + "/Videos/" + v.topic_id;
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            string filepath = folderpath + "/" + filename;
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            //            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            video.CopyTo(fs);
            v.video_url = filename;
           await topicService.AddTopicVideo(v);
            ViewBag.videos =await topicService.GetAllTopicVideos();
            return Redirect("/Developer/Topic/Video");
        }


        [HttpPost]
        public async Task<string> AddTopicFolder([FromBody]TopicModel tp)
        {
           await topicService.AddTopicFolderId(tp);
            return "Folder id added successfully";
        }
    }
}
