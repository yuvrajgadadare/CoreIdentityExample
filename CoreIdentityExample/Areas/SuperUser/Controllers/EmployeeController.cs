using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CoreIdentityExample.Areas.SuperUser.Controllers
{
    [Area("SuperUser")]
    public class EmployeeController : Controller
    {
        IEmployeeService employeeService;
        private readonly UserManager<ApplicationUser> userManager;
        //signInManager will hold the SignInManager instance
        private readonly SignInManager<ApplicationUser> signInManager;
        IExtraService extraService;
        IBranchService branchService;
        IWebHostEnvironment _env;
        EmailSettings _settings;

        public EmployeeController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IEmployeeService employeeService, IExtraService extraService, IWebHostEnvironment env, IBranchService branchService, IOptions<EmailSettings> settings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.employeeService = employeeService;
            this.extraService = extraService;
            this._env = env;
            this.branchService = branchService;
            this._settings=settings.Value;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);
            ViewData["employees"]=await employeeService.GetEmployees(emp.branch_id);
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee(EmployeeRegistrationModel model, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,


                };
                string password =await extraService.GetRandomPassword(10);
                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, password);
                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {




                    EmployeeModel em = new EmployeeModel()
                    {
                        user_id = user.Id,
                        employee_name = model.FirstName + " " + model.LastName,
                        joining_date = model.joining_date,
                        email_address = model.Email,
                        employee_code = await employeeService.NextEmployeeCode(),
                        birth_date = model.birth_date,
                        gender = model.gender,
                         qualification = model.qualification,
                        mobile_number = model.mobile_number,
                         branch_id = model.branch_id


                    };
                    Random r = new Random();
                    string imgname = em.employee_code + r.Next(1000, 100000) + Path.GetExtension(photo.FileName);
                    string imgpath = _env.WebRootPath + "/Employees/" + imgname;
                    FileStream fs = new FileStream(imgpath, FileMode.Create);
                    photo.CopyTo(fs);
                    em.profile_photo = imgname;
                    await employeeService.AddEmployeeDetails(em);
                    string message = "<h4>Dear "+em.employee_name+",</h4><p>Your have successully registered on ciit's erp portal.</p><p>You can access your account with following login details.</p><p>Email address: <b>"+em.email_address+"</b> and Password: <b>"+password+ "</b></p><br/><br/><br/><br/><br/><h4>Regards,<br/>CIIT Training Institute Pvt Ltd.</h4>";
                    EmailModel email = new EmailModel()
                    {
                        UserName = em.employee_name,
                        EmailAddress = em.email_address,
                        Subject = "Account registration confirmation",
                        Message = message
                    };
                    extraService.SendEmail(email,_settings);
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    ViewBag.msg = "employee added successfully";
                    ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");

                    return View();
                }
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");

            return View(model);
        }



        public async Task<JsonResult> GetEmployee(int id)
        {
            
            EmployeeModel employee = await employeeService.GetEmployee(id);
            return Json(employee);
        }
        [HttpPost]
        public async Task<IActionResult> Index(EmployeeModel e)
        {

           await employeeService.UpdateEmployeeDetails(e);
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);
            ViewData["employees"] = await employeeService.GetEmployees(emp.branch_id);
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            return View();
       
        }
    }
}
