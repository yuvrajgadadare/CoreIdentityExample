using ERP_Models;
using ERP_Services.Interfaces;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Data;
using System.Security.Claims;
using System.Text;

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
        EmailSettings _settings;
        IWebHostEnvironment _env;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IEmployeeService employeeService, IExtraService extraService,IWebHostEnvironment env,IOptions<EmailSettings> emailSettings)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.employeeService = employeeService;
            this.extraService = extraService;
            this._env = env;
            this._settings=emailSettings.Value;
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
                string password = await extraService.GetRandomPassword(10);
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
                         gender=model.gender,
                          //qualification=model.qualification,
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
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ApplicationUser user=await userManager.FindByNameAsync(model.Email);
                    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);
               //     if (await userManager.IsInRoleAsync(user, "Administrator"))
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login"); // User not found, redirect to login
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (changePasswordResult.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been changed successfully.";
                return RedirectToAction("ChangePasswordConfirmation");
            }

            foreach (var error in changePasswordResult.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            //return View(model);
            //if (changePasswordResult.Succeeded)
            //{
            //    // Update the security stamp to invalidate old cookies and sign the user in with a new cookie
            //    await userManager.UpdateSecurityStampAsync(user);
            //    await signInManager.SignInAsync(user, isPersistent: false);
            //    ViewBag.msg = "Your password has been changed.";
            //    return View();
            //}

            //// If the action fails, add errors to the model state and return the view
            //foreach (var error in changePasswordResult.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, error.Description);
            //}

            return View(model);
        }
        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel em)
        {

            if (await SendPasswordResetLinkAsync(em.EmailAddress))
            {
                return View("ForgotPasswordConfirmation");
            }
            else
            {
                ViewBag.Message = "employee with given email address is not present";
                return View();
            }
            }
        public async Task<bool> SendPasswordResetLinkAsync(string email)
        {
            // Try to find the user by their email address
            var user = await userManager.FindByEmailAsync(email);
            // Security measure: 	
             
       // Do not reveal whether the user exists or not — 
       // always behave the same if the user is not found or the email is not confirmed
    if (user == null)
                return false;
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedEmail = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(user.Email));
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            // Construct the password reset link with the encoded token and user’s email
            // var baseUrl = _configuration["AppSettings:BaseUrl"];
            var baseUrl = "https://ciitstudent.com";
            var resetLink = $"{baseUrl}/Account/ResetPassword?email={encodedEmail}&token={encodedToken}";
            // Send the reset link via email to the user
            string html = $@"
            <html><body style='font-family: Arial, sans-serif; background:#f4f6f8; margin:0; padding:20px;'>
              <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                <h2 style='color:#333;'>Password Reset Request</h2>
                <p style='font-size:16px; color:#555;'>Hi {user.UserName},</p>
                <p style='font-size:16px; color:#555;'>We received a request to reset your password. Click the button below to choose a new one.</p>
                <p style='text-align:center;'>
                  <a href='{resetLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Reset Password</a>
                </p>
                <p style='font-size:13px; color:#777;'>If you didn't request this, you can ignore this email.</p>
                <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} Dot Net Tutorials. All rights reserved.</p>
              </div>
            </body></html>";
            EmailModel em = new EmailModel() { UserName = user.UserName, EmailAddress = user.Email, Subject = "Forgot password link", Message = html };

            await extraService.SendEmail(em,_settings);
            return true;
        }
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> ResetPassword(string email, string token)
        {

          
            
            //  var email_address = WebEncoders.Base64UrlDecode(email);

            var user = await userManager.FindByEmailAsync(email);
            ResetPasswordViewModel rm=new ResetPasswordViewModel() {  Email= email, Token=token };
            return View(rm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            byte[] decodedemail = WebEncoders.Base64UrlDecode(model.Email);
            string email_address = Encoding.UTF8.GetString(decodedemail);
          
            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var user = await userManager.FindByEmailAsync(email_address);
            var result = await userManager.ResetPasswordAsync(user,decodedToken,model.Password);
            if (result.Succeeded)
                return View("ResetPasswordConfirmation");
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

    }
}
