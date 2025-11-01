using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreIdentityExample.Areas.Developer.Controllers
{
    [Area("Developer")]

    public class ProgramController : Controller
    {
        IQuestionService questionService;
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        public ProgramController(IQuestionService questionService, IMasterService masterService, ITopicService topicService, IContentService contentService)
        {
            this.questionService = questionService;
            this.masterService = masterService;
            this.topicService = topicService;
            this.contentService = contentService;
        }
        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ProgramQuestionModel im = new ProgramQuestionModel();
            return View();
        }
        public async Task<IActionResult> AddProgramQuestionWithAnswer()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            ViewBag.contents = new SelectList(await contentService.GetTopicWiseContents(0), "content_id", "content_name");
            ProgramAnswerModel im = new ProgramAnswerModel();

            return View(im);
        }
        [HttpPost]
        public async Task<IActionResult> AddProgramQuestionWithAnswer(ProgramAnswerModel p)
        {
           await questionService.AddProgramQuestionWithAnswer(p);
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
          ModelState.Clear();
            ViewBag.topics = new SelectList( await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            ViewBag.contents = new SelectList(await contentService.GetTopicWiseContents(0), "content_id", "content_name");
            ProgramAnswerModel im = new ProgramAnswerModel();
            return View(im);
        }
        public async Task<IActionResult> AddProgramQuestion()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            ViewBag.topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");

            ProgramQuestionModel im = new ProgramQuestionModel();

            return View(im);
        }

        [HttpPost]
        public async Task<string> AddProgramQuestions(ProgramFormModel q)
        {
            foreach (ProgramQuestionModel p in q.questions)
            {
                questionService.AddProgramQuestion(p);
            }
            return "Added Successfully";
        }

        public async Task<JsonResult> GetQuestions()
        {
            List<ProgramQuestionModel> lst = await questionService.GetAllProgramQuestions();
            return Json(lst);
        }
        public async Task<JsonResult> GetTopicWiseQuestions(int id)
        {
            List<ProgramQuestionModel> lst = await questionService.GetTopicWiseProgramQuestions(id);
            return Json(lst);
        }
        public async Task<JsonResult> GetContentWiseQuestions(int id)
        {
            List<ProgramQuestionModel> lst =await questionService.GetContentWiseProgramQuestions(id);
            return Json(lst);
        }


        public async Task<IActionResult> ProgramAnswer()
        {
            List<ProgramQuestionModel> lst = await questionService.GetAllProgramQuestions();
            ViewBag.programs=new SelectList(lst, "program_question_id","question_title");
            ViewData["programsanswers"] = questionService.GetAllProgramAnswers();
            ProgramAnswerModel pm = new ProgramAnswerModel();
            return View(pm);
        }
        [HttpPost]
        public async Task<IActionResult> ProgramAnswer(ProgramAnswerModel p)
        {
           await questionService.AddProgramAnswer(p);
            ModelState.Clear();
            ViewBag.msg = "Program answer added successfully";
            List<ProgramQuestionModel> lst =await questionService.GetAllProgramQuestions();
            ViewBag.programs = new SelectList(lst, "program_question_id", "question_title");
            ViewData["programsanswers"] =await questionService.GetAllProgramAnswers();

            ProgramAnswerModel pm = new ProgramAnswerModel();
            return View(pm);
        }





    }
}
