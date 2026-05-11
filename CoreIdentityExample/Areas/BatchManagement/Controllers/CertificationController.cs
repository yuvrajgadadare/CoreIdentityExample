using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.Extensions.Options;

namespace CoreIdentityExample.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]
    [Authorize]
    public class CertificationController : Controller
    {
        IMasterService masterService;
        IStudentService studentService;
        IExtraService extraService;
        EmailSettings _settings;
        IBatchService batchService;
        IContentService contentService;
        ITopicService topicService;
        IExamService examService;
        IEmployeeService employeeService;
        IBranchService branchService;
        public static int currentPage = 0;
        ICacheService _cacheService;

        public CertificationController(IMasterService masterService, IStudentService studentService, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService, IContentService contentService, ITopicService topicService, IExamService examService, IEmployeeService employeeService,IBranchService branchService, ICacheService cacheService)
        {
            this.masterService = masterService;
            this.studentService = studentService;
            this.extraService = extraService;
            this.batchService = batchService;
            _settings = settings.Value;
            this.contentService = contentService;
            this.topicService = topicService;
            this.examService = examService;
            this.employeeService = employeeService;
            this.branchService = branchService;
            this._cacheService = cacheService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.branches=new SelectList(await branchService.GetBranches(),"branch_id","branch_name");
            return View();
        }

        public async Task<JsonResult> GetBranchWiseStudents(int id)
        { 
            List<StudentModel> lst = await studentService.GetStudents(id);
            return Json(lst); 
        }
        public async Task<JsonResult> GetStudentExams(int id)
        {
            List<ExamModel> lst = await examService.GetStudentRegistrationWiseSubmittedExams(id);
            return Json(lst);
        }
        public async Task<IActionResult> ViewCertificate(int id)
        {
           StudentCertificationModel s=await examService.GetStudentCertificate(id);
            List<ExamModel> lst = await examService.GetStudentRegistrationWiseSubmittedExams(id);
            ViewData["exams"] = lst;
            return View(s);
        }
        //public async Task<JsonResult> GetBranchWiseStudents(int id)
        //{
        //    var cacheData = _cacheService.GetData<IEnumerable<StudentModel>>("students");
        //    if (cacheData != null)
        //    {
        //        return Json(cacheData);
        //    }
        //    var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        //    // cacheData = _dbContext.Products.ToList();
        //    List<StudentModel> lst = await studentService.GetStudents(id);
        //    _cacheService.SetData<IEnumerable<StudentModel>>("students", lst, expirationTime);
        //    return Json(lst);
        //}
        //public async Task<JsonResult> GetStudentExams(int id)
        //{
        //    List<ExamModel> lst = await examService.GetStudentWiseExams(id);
        //    return Json(lst);

        //    var cachedata = _cacheservice.getdata<ienumerable<exammodel>>("exams");
        //    if (cachedata != null)
        //    {
        //        return json(cachedata);
        //    }
        //    var expirationtime = datetimeoffset.now.addminutes(5.0);
        //    // cachedata = _dbcontext.products.tolist();
        //    list<exammodel> lst = await examservice.getstudentwiseexams(id);
        //    _cacheservice.setdata<ienumerable<exammodel>>("exams", lst, expirationtime);
        //    return json(lst);
        //}
    }
}
