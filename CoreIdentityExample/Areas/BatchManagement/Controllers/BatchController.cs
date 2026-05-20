
using ERP_Models;
using ERP_Services.Interfaces;
 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ERP_Services.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
    public class BatchController : Controller
    {
        IBatchService batchService;
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        IEmployeeService employeeService;
        IExamService examService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public static  int branch_id = 0;
        public BatchController(RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager, IBatchService batchService, IMasterService masterService, ITopicService topicService, IContentService contentService,IEmployeeService employeeService,IExamService examService)
        {
            this.batchService = batchService;
            this.masterService = masterService;
            this.topicService = topicService;
            this.contentService = contentService;
            this._roleManager=_roleManager;
            this._userManager = _userManager;
            this.employeeService = employeeService;
            this.examService = examService;

        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel employee = await employeeService.GetEmployeeByUserId(userId);
            branch_id = employee.branch_id;
            BatchModel b =new BatchModel();
            List<UserRoleViewModel> users=new List<UserRoleViewModel>();
            List<EmployeeModel> lst = new List<EmployeeModel>();
            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.FirstName+" "+user.LastName,
                    
                };

                if (await _userManager.IsInRoleAsync(user, "Trainer"))
                {
                    users.Add(userRoleViewModel);
                    EmployeeModel emp =await employeeService.GetEmployeeByUserId(user.Id);
                    lst.Add(emp);
                }

               
            }
            
            SelectList topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            //SelectList trainers = new SelectList(users, "UserId", "UserName");
            SelectList trainers = new SelectList(lst, "employee_id", "employee_name");

            
            List<BatchModel> batches=  await batchService.GetAllBatches(branch_id);
            List<BatchModel> batchlist =new List<BatchModel>();
            foreach (var batch in batches) {

                List<BatchExamModel> exams = await examService.GetBatchWiseScheduledStudentExams(batch.batch_id);
                if (exams.Count > 0)
                {
                    batch.is_schedule_exams_generated = true;
                }
                else
                {
                    batch.is_schedule_exams_generated = false;
                }
                batchlist.Add(batch);

            }
            ViewBag.topics = topics;
            ViewBag.trainers = trainers;
            ViewData["batches"]  = batchlist;
           // int branch_id = (int)HttpContext.Session.GetInt32("branch_id");

            ViewData["deletedbatches"]  = await batchService.GetAllDeletedBatches(branch_id);
            return View(b);
        }
       

        [HttpPost]
        public async Task<string>  CreateNewBatch(BatchModel batch)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");

            //EmployeeModel emp = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //ViewData["employee"] = emp;
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel employee = await employeeService.GetEmployeeByUserId(userId);
            branch_id = employee.branch_id;
            batch.branch_id = branch_id;
            await  batchService.AddBatch(batch);
          //  ModelState.Clear();
          return "Batch Created Successfully";
            //BatchModel b = new BatchModel();

            //SelectList topics = new SelectList(await topicService.GetTrainingTopics(), "topic_id", "topic_name");
            //SelectList trainers = new SelectList(await batchService.GetAllTrainers(), "employee_id", "employee_name");
            //ViewBag.topics = topics;
            //ViewBag.trainers = trainers;
            //ViewData["batches"] = batchService.GetAllBatches().Result;
            //return View(b);
        }
        //public async Task<IActionResult> DeleteBatch(int id)
        //{
        //    batchService.DeleteBatch(id);

        //    return RedirectToAction("Index");
        //}
        public async Task<string> DeleteBatch(int id)
        {
            await batchService.DeleteBatch(id);

            return "Batch removed successfully";
        }
        public async Task<string> RestoreBatch(int id)
        {
            await batchService.RestoreBatch(id);

            return "Batch restored successfully";
        }
        public async Task<JsonResult> GetBatchWiseStudents(int id)
        {
            List<BatchStudentModel> lst = await batchService.GetBatchWiseStudents(id);
            return Json(lst);
        }

        [HttpPost]
        public async Task<string> GenerateBatchSchedule(int id)
            {
            BatchModel batch =await batchService.GetBatch(id);
            List<ContentModel> contents =await contentService.GetTopicWiseContents(batch.topic_id);
            DateTime dt =batch.start_date;
            foreach (ContentModel content in contents)
            {
               
                BatchScheduleModel bs = new BatchScheduleModel() 
                {
                 batch_id=batch.batch_id,
                 content_id=content.content_id,
                  expected_date=dt
                };
                batchService.AddBatchSchedule(bs);

                if(dt.DayOfWeek == DayOfWeek.Friday)
                {
                    dt = dt.AddDays(3);
                }
                else 
                {
                    dt = dt.AddDays(1);
                }
                

            }
            return "Schedule Generated Successfully";
        }
        public async Task<JsonResult> GetTopicWiseStudents(int id)
        {
            List<TopicStudentModel> lst =await batchService.GetTopicWiseStudents(id);
            return Json(lst);
        }
        public async Task<IActionResult> ViewBatchSchedule(int id)
        {
           
            List<BatchScheduleModel>schedule=await batchService.GetBatchWiseSchedule(id);
            List<BatchStudentModel> students =await batchService.GetBatchWiseStudents(id);
            List<BatchScheduleExamModel> exams =await batchService.GetBatchWiseScheduledExams(id);
            BatchModel b =await batchService.GetBatch(id);

            ViewData["batch"] = b;

            ViewData["students"] = students;
            ViewData["exams"] = exams;
            return View(schedule);
        }

        [HttpPost]
        public async Task<string> AddBatchStudents(BatchStudents b)
        {

            string registrations = b.registration_ids.Substring(1, b.registration_ids.Length - 1);
            string[] data = registrations.Split(",");
            foreach(string s in data) { 
            int reg_id=Convert.ToInt32(s);
                BatchStudentModel bsmodel = new BatchStudentModel() 
                {
                 batch_id=b.batch_id,
                  registration_id=reg_id
                };
              await  batchService.AddBatchStudent(bsmodel);

            }
            return "Students Added Successfully";
        }
        [HttpPost]
        public async Task<string> GenerateBatchExams(int batch_id, int total_questions)
        {
            List<BatchExamModel> exams = await examService.GetBatchWiseScheduledStudentExams(batch_id);
            if (exams.Count > 0)
            {
                return "Exams are already scheduled";
            }
            else
            {
                examService.GenerateBatchExams(batch_id, total_questions);
                return "Batch Exams generated successfully";
            }
        }

        [HttpPost]
        public async Task<string> SetPlayListKey([FromBody]BatchPlayListModel b)
        {
            await batchService.SetPlayListTitle(b);
            return "Play list key set successfully"; 
        }
    }
}
