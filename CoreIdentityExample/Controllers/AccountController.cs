using ERP_Models;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace CoreIdentityExample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        //signInManager will hold the SignInManager instance
        private readonly SignInManager<ApplicationUser> signInManager;
        //Both UserManager and SignInManager services are injected into the AccountController
        //using constructor injection
        IEmployeeService employeeService;
        IExtraService extraService;

        IWebHostEnvironment _env;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IEmployeeService employeeService, IExtraService extraService,IWebHostEnvironment env)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.employeeService = employeeService;
            this.extraService = extraService;
            this._env = env;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> IsEmailAvailable(string Email)
        {
            //Check If the Email Id is Already in the Database
            var user = await userManager.FindByEmailAsync(Email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {Email} is already in use.");
            }
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(EmployeeRegistrationModel model, IFormFile photo)
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
                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);
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
                         gender=model.gender,
                          qualification=model.qualification,
                          mobile_number=model.mobile_number
                         

                    };
                    Random r = new Random();
                    string imgname = em.employee_code + r.Next(1000, 100000) + Path.GetExtension(photo.FileName);
                    string imgpath = _env.WebRootPath + "/Employees/" + imgname;
                    FileStream fs = new FileStream(imgpath, FileMode.Create);
                    photo.CopyTo(fs);
                    em.profile_photo = imgname;
                    await employeeService.AddEmployeeDetails(em);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        //public async Task<IActionResult> Register(RegisterViewModel model,IFormFile photo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Copy data from RegisterViewModel to ApplicationUser
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,

        //        };
        //        // Store user data in AspNetUsers database table
        //         var result = await userManager.CreateAsync(user, model.Password);
        //        // If user is successfully created, sign-in the user using
        //        // SignInManager and redirect to index action of HomeController
        //        if (result.Succeeded)
        //        {




        //            EmployeeModel em = new EmployeeModel()
        //            {
        //                user_id = user.Id,
        //                employee_name = model.FirstName + " " + model.LastName,
        //                joining_date = DateTime.Now,
        //                email_address = model.Email,
        //                employee_code = await employeeService.NextEmployeeCode(),
        //                birth_date = DateTime.Now

        //            };
        //            Random r = new Random();
        //            string imgname = em.employee_code + r.Next(1000, 100000) + Path.GetExtension(photo.FileName);
        //            string imgpath = _env.WebRootPath + "/Employees/" + imgname;
        //            FileStream fs = new FileStream(imgpath, FileMode.Create);
        //            photo.CopyTo(fs);
        //            em.profile_photo = imgname;
        //            await employeeService.AddEmployeeDetails(em);
        //            await signInManager.SignInAsync(user, isPersistent: false);
        //            return RedirectToAction("index", "home");
        //        }
        //        // If there are any errors, add them to the ModelState object
        //        // which will be displayed by the validation summary tag helper
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }
        //    return View(model);
        //}
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            // Handle successful login
        //            return RedirectToAction(nameof(HomeController.Index), "Home");
        //        }
        //        if (result.RequiresTwoFactor)
        //        {
        //            // Handle two-factor authentication case
        //        }
        //        if (result.IsLockedOut)
        //        {
        //            // Handle lockout scenario
        //        }
        //        else
        //        {
        //            // Handle failure
        //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //            return View(model);
        //        }
        //    }
        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Login(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ApplicationUser user=await userManager.FindByNameAsync(model.Email);
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);
                    
                    if (await userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        return Redirect("/Developer/Dashboard/Index");
                    }
                    // Handle successful login
                    // Check if the ReturnUrl is not null and is a local URL
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        // Redirect to default page
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (result.RequiresTwoFactor)
                {
                    // Handle two-factor authentication case
                }
                if (result.IsLockedOut)
                {
                    // Handle lockout scenario
                }
                else
                {
                    // Handle failure
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Profile()
        {
            string uname = User.Identity.Name;
            ViewBag.user = uname;
            string userId=userManager.GetUserId(User);
            EmployeeModel employee=await employeeService.GetEmployeeByUserId(userId);
            if (employee == null)
            {


                ClaimsPrincipal currentUser = this.User;

                // Get the IdentityUser object for the current user
                ApplicationUser applicationUser = await userManager.GetUserAsync(currentUser);

                EmployeeModel emp = new EmployeeModel()
                {
                    user_id = userId,
                    email_address = applicationUser.Email,
                    employee_name = applicationUser.FirstName + " " + applicationUser.LastName,
                };
                ViewBag.status = false;
                return View(emp);
            }
            else
            {
                ViewBag.status = true;

                return View(employee);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Profile(EmployeeModel e,IFormFile photo)
        {
            if (e.employee_code == null)
            {
                e.employee_code=await employeeService.NextEmployeeCode();
            }
            Random r=new Random();
            string imgname = e.employee_code + r.Next(1000, 100000) + Path.GetExtension(photo.FileName);
            string imgpath = _env.WebRootPath + "/Employees/" + imgname;
            FileStream fs = new FileStream(imgpath, FileMode.Create);
            photo.CopyTo(fs);
            e.profile_photo = imgname;
            await employeeService.AddEmployeeDetails(e);
            return View(e);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
