using Microsoft.AspNetCore.Mvc;
using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]
    public class CourseController : Controller
    {
       IMasterService masterService;
        ICourseService courseService;
        ITopicService topicService;
        IContentService contentService;
        public CourseController(IMasterService masterService, ICourseService courseService, ITopicService topicService, IContentService contentService)
        {
            this.masterService = masterService;
            this.courseService = courseService;
            this.topicService = topicService;
            this.contentService = contentService;
        }
        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            List<CourseModel> courses =await  topicService.GetTrainingCoursesWithTopics();
            return View(courses);
        }

        public async Task<JsonResult> GetTopics()
        {
            return Json(await topicService.GetTrainingTopics());
        }
        public async Task<JsonResult> GetCourseTopics(int id)
        {
            return Json(await topicService.GetCourseWiseTopics(id));
        }

        public async Task<JsonResult> GetCourses()
        {
            return Json(await courseService.GetTrainingCourses());
        }
        public async Task<JsonResult> GetCoursesForDropdown()
        {
            SelectList s = new SelectList(await courseService.GetTrainingCourses(),"course_id","course_name");
            return Json(s);
        }
        [HttpPost]
        public async Task<string> AddCourseDetails(CourseModel cm)
        {
           await courseService.AddTrainingCourse(cm);
            return "Course Added Successfully";
        }
        [HttpPost]
        public async Task<string> DeleteCourseTopic(int id)
        {
            await courseService.DeleteCourseTopic(id);
            return "Course Topic Deleted Successfully";
        }
        [HttpPost]
        public async Task<string> AddCourseTopics(CourseModel course)
        {
           await topicService.AddCourseTopics(course);
            return "Course Topics Added Successfully";
        }
        public async Task<IActionResult> CourseSyllabus(int course_id)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //CourseModel cm=await courseService.GetTrainingCourse(course_id);
            CourseModel cm = await contentService.GetTrainingCourse(course_id);

            return View(cm);
        }

        [HttpPost]
        public async Task<string> AddLatestFees(CourseFeeModel fee)
        {
          await courseService.AddCourseFees(fee);
            return "Course  fees added successfully";
        }

        [HttpPost]
        public string DeleteCourse(int id)
        {
            courseService.DeleteCourse(id);
            return "Course Deleted Successfully";
        }
    }
}
