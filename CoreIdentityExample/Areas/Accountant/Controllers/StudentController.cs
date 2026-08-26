using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System;
using System.Security.Claims;

namespace CoreIdentityExample.Areas.Accountant.Controllers
{
    [Area("Accountant")]
    [Authorize]
    public class StudentController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        ICourseService courseService;
        IEmployeeService employeeService;
        IBranchService branchService;
        public StudentController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService,IOptions<EmailSettings> settings, IBatchService batchService, ICourseService courseService, IEmployeeService employeeService,IBranchService branchService)
        {
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
            this.courseService = courseService;
            this.employeeService = employeeService;
            this.branchService = branchService;
        }
        public async Task<bool> IsEmailExist(string email_address)
        {
            bool result =await studentService.IsEmailExist(email_address);
            return result;
        }
        public async Task<bool> IsMobileExist(string mobile_number)
        {
            bool result = await studentService.IsMobileExist(mobile_number);
            return result;
        }
        public async Task<IActionResult> RegistrationForm()
        {
            // ViewData["students"] = studentService.GetStudents();
            //ViewBag.courses =await GetCoursesForGuest();
            ViewData["courses"] = await courseService.GetCourseFees();
            ViewBag.branches  = await branchService.GetAllBranches();
            string pinnumber =await studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
        }
         public async Task<List<SelectListItem>> GetFees()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (CourseFeeModel f in await courseService.GetCourseFees())
            {
                SelectListItem sm = new SelectListItem()
                {
                    Text = f.course_name + " (" + f.fee_mode + "-" + f.fees_amount + ")",
                    Value = f.fee_id.ToString()
                };
                lst.Add(sm);
            }
            return lst;
        }
        public async Task<JsonResult> GetAllCourses()
        {
            return Json(await courseService.GetCoursesWithMinimumFees());
        }
        public async Task<CourseFeeModel> GetCourseFee(int id)
        {
            CourseFeeModel f = await courseService.GetCourseFee(id);
            return f;
        }

        public async Task<List<SelectListItem>> GetQualifications()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (QualificationModel f in await masterService.GetQualifications())
            {
                SelectListItem sm = new SelectListItem()
                {
                    Text = f.qualification,
                    Value = f.qualification
                };
                lst.Add(sm);
            }
            return lst;
        }
        [HttpPost]
        public async Task<ActionResult> RegistrationForm(StudentModel d, IFormFile photo, IFormFile aadharcard, string feeid)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);

            Random r = new Random();
            int n = r.Next(1, 1000);
            d.student_code = "Guest";
            string imgname = d.student_name + "_" + n + Path.GetExtension(photo.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            photo.CopyTo(fs);
            d.profile_photo = imgname;

            string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            if (System.IO.File.Exists(aadharpath))
            {
                System.IO.File.Delete(aadharpath);
            }
            FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            aadharcard.CopyTo(fsaadhar);
            d.aadhar_card_photo = aadharname;



            string password =await extraService.GetRandomPassword(10);
            d.password = password;
            RegistrationModel rs = new RegistrationModel()
            {
                registration_date = DateTime.Now,
                fee_id = Convert.ToInt32(feeid),
                discount = 0
            };
            List<RegistrationModel> registrations = new List<RegistrationModel>();
            registrations.Add(rs);
            d.registrations = registrations;
          await  studentService.AddStudentRegistration(d);
            string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
            EmailModel em = new EmailModel() { UserName = d.student_name, EmailAddress = d.email_address, Message = message, Subject = "Registration Confirmation" };
          await  extraService.SendEmail(em, _settings);
            //ModelState.Clear();
            //ViewBag.msg = "Student Added Successfully";
            //ViewBag.courses = GetCourses();
            //ViewBag.students = studentService.GetStudentRegistrations();
            //ViewData["students"] = studentService.GetStudents();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            // ViewData["students"] = studentService.GetStudents();
            ModelState.Clear();
            ViewBag.msg = "Student Added Successfully";
            ViewData["students"] =await studentService.GetStudents(emp.branch_id);
            ViewData["guest"] =await studentService.GetGuestStudents(emp.branch_id);
            ViewData["courses"] = await courseService.GetCourseFees();

            //ViewBag.courses = GetCourses();
            string pinnumber =await studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
            //return View(sm);
        }
        public async Task<List<SelectListItem>> GetCoursesForGuest()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach (CourseFeeModel c in await courseService.GetCourseFees())
            {
                SelectListItem st = lst.FirstOrDefault(e => e.Text.Equals(c.course_name));
                if (st == null)
                {
                    SelectListItem s = new SelectListItem() { Text = c.course_name, Value = c.fee_id.ToString() };
                    lst.Add(s);
                }
            }
            return lst;
        }

        [HttpPost]
        public async Task<string> ChangeStudentCourse(RegistrationModel r)
        {
           await studentService.ChangeStudentCourse(r);
            return "Course changed successfully";
        }

        public  async Task< List<SelectListItem>>  GetCourses()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            foreach(CourseFeeModel c in await courseService.GetCourseFees())
            {
                SelectListItem s = new SelectListItem() { Text=c.course_name+"("+c.fee_mode+"-"+c.fees_amount+")", Value=c.fee_id.ToString() };
                lst.Add(s);
            }
            return lst ;
        }
        public async Task<IActionResult> StudentRegistrationForm()
        {
            ViewBag.courses = await GetFees();
            ViewBag.qualifications = await GetQualifications();
            ViewBag.branches = new SelectList(await branchService.GetAllBranches(), "branch_id", "branch_name");
            //ViewData["qualifications"] = await courseService.GetTrainingCourses();
            StudentModel sm = new StudentModel() { };
            return View(sm);
        }

        [HttpPost]
        public async Task<String> SubmitStudentRegistrationForm([FromBody]StudentModel sm)
        {
            string pinnumber = await studentService.NextPINNumber();
            sm.permanent_identification_number = pinnumber;
            string password = await extraService.GetRandomPassword(10);
            sm.password = password;
            sm.student_code = "Student";
           string msg= await  studentService.AddStudentRegistration(sm);
            if (msg == "1")
            {
                string message = "<h2>Dear<br/> " + sm.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + sm.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
                EmailModel em = new EmailModel() { UserName = sm.student_name, EmailAddress = sm.email_address, Message = message, Subject = "Registration Confirmation" };
                await extraService.SendEmail(em, _settings);
                return "Student Registered Successfully";
            }
            else
            {
                return msg;
            }
        }
        public async Task<IActionResult> NewRegistration()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //ViewData["students"] = await studentService.GetStudents();
            //ViewData["guest"] = await studentService.GetGuestStudents();
            //ViewBag.years = await extraService.GetYears();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            //return View(sm);
            ViewData["courses"] = await courseService.GetTrainingCourses();
            string pinnumber = await studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
        }
        [HttpPost]
        public async Task<ActionResult> NewRegistration(StudentModel d, IFormFile photo, IFormFile aadharcard, DateTime registration_date, string feeid)
        {
            //if(HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            bool emailexist = await studentService.IsEmailExist(d.email_address);
            if (emailexist)
            {
                ViewBag.msgemail = "email address is already registered";
                ViewData["courses"] = await courseService.GetTrainingCourses();
                string pinnumber1 = await studentService.NextPINNumber();
                StudentModel smd = new StudentModel() { permanent_identification_number = pinnumber1 };
                return View(smd);
            }
            bool mobileexist = await studentService.IsMobileExist(d.mobile_number);
            if (mobileexist)
            {
                ViewBag.msgmobile = "mobile number is already registered";

                ViewData["courses"] = await courseService.GetTrainingCourses();
                string pinnumber1 = await studentService.NextPINNumber();
                StudentModel smd = new StudentModel() { permanent_identification_number = pinnumber1 };
                return View(smd);
            }

            Random r = new Random();
            int n = r.Next(1, 1000);
            d.student_code = "Student";
            string imgname = d.student_name + "_" + n + Path.GetExtension(photo.FileName);
            string imgpath = environment.WebRootPath + "/Students/Profiles/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            photo.CopyTo(fs);
            d.profile_photo = imgname;

            string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            if (System.IO.File.Exists(aadharpath))
            {
                System.IO.File.Delete(aadharpath);
            }
            FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            aadharcard.CopyTo(fsaadhar);
            d.aadhar_card_photo = aadharname;



            string password = await extraService.GetRandomPassword(10);
            d.password = password;
            RegistrationModel rs = new RegistrationModel()
            {
                //registration_date = DateTime.Now,
                registration_date = registration_date,
                fee_id = Convert.ToInt32(feeid),
                discount = 0
            };
            List<RegistrationModel> registrations = new List<RegistrationModel>();
            registrations.Add(rs);
            d.registrations = registrations;
            await studentService.AddStudentRegistration(d);
            //      string message = "<h2>Dear<br/> " + d.student_name + ",</h2><p>Your Account has been Created successfully.You can refer <a href='https://ciitstudent.com/' target='_blank'>ciitstudent.com</a> login by email address  <b>" + d.email_address + "</b> and password <b>" + password + "</b></p><br/><br/><h4>Regards,CIIT Training Institute Pvt. Ltd.</h4>";
            //    EmailModel em = new EmailModel() { UserName = d.student_name, EmailAddress = d.email_address, Message = message, Subject = "Registration Confirmation" };
            //    extraService.SendEmail(em, _settings);
            //ModelState.Clear();
            //ViewBag.msg = "Student Added Successfully";
            //ViewBag.courses = GetCourses();
            //ViewBag.students = studentService.GetStudentRegistrations();


            //ViewData["students"] = studentService.GetStudents();
            //ViewBag.courses = GetCourses();
            //StudentModel sm = new StudentModel();
            // ViewData["students"] = studentService.GetStudents();
            ModelState.Clear();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            ViewBag.msg = "Student Added Successfully";
            ViewData["students"] = await studentService.GetStudents(emp.branch_id);
            ViewData["guest"] = await studentService.GetGuestStudents(emp.branch_id);
            //ViewBag.courses = GetCourses();
            ViewData["courses"] = await courseService.GetTrainingCourses();
            ViewBag.years = await extraService.GetYears();

            string pinnumber = await studentService.NextPINNumber();
            StudentModel sm = new StudentModel() { permanent_identification_number = pinnumber };
            return View(sm);
            //return View(sm);

        }

        public async Task<IActionResult> Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<StudentModel> lst= await studentService.GetStudents(emp.branch_id);
            ViewBag.years =await extraService.GetYears();
            //ViewData["courses"] =await masterService.GetTrainingCourses();
            return View(lst);
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(int year)
        //{
        //    //if (HttpContext.Session.GetString("employee") == null)
        //    //{
        //    //    return Redirect("/Account/Login");
        //    //}
        //    var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


        //    List<StudentModel> lst = await studentService.GetStudents(emp.branch_id);
        //    ViewBag.years = await extraService.GetYears();
        //    //ViewData["courses"] =await masterService.GetTrainingCourses();
        //    return View(lst);
        //}
        public async Task<JsonResult> GetAllStudents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<StudentModel> lst = await studentService.GetStudents(emp.branch_id);
            ViewBag.years = await extraService.GetYears();
            //ViewData["courses"] =await masterService.GetTrainingCourses();
            return Json(lst);
        }
        [HttpPost]
        public async Task<IActionResult> Index(int year)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<StudentModel> lst = await studentService.GetYearAndBranchWiseStudents(emp.branch_id, year);
            ViewBag.years = await extraService.GetYears();
            //ViewData["courses"] =await masterService.GetTrainingCourses();
            ViewBag.result =  year;
            return View(lst);
        }

        public async Task<IActionResult> GetStudentWisedetails(int id)
        {
            StudentModel sm=await studentService.GetStudent(id);
            List<RegistrationModel> registrationlist= await studentService.GetStudentWiseRegistrations(id);
            RegistrationModel rm = registrationlist.OrderByDescending(e => e.registration_id).First();
            List<BatchStudentModel> batches =await batchService.GetStudentWiseBatches(id);
            ViewData["batches"] = batches;
            ViewData["payments"] = await studentService.GetStudentWisePayments(id);
        
            ViewData["pendingpayments"] =await studentService.GetStudentsWiseRemainingPayments(rm.registration_id);
            //ViewBag.courses = GetCourses();
            ViewData["courses"] = await courseService.GetTrainingCourses();

            return View(sm);
        }
        public async Task<IActionResult> PayFees()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);

            List<RegistrationModel>lst=await studentService.GetAllRegistrations(emp.branch_id);
            StudentPaymentModel sm = new StudentPaymentModel();
            //ViewBag.registrations = new SelectList(lst,"registration_id","student_name");
            ViewBag.registrations = await GetRegisteredStudents();

            return View(sm);
        }
        [HttpPost]
        public async Task<IActionResult> PayFees(StudentPaymentModel s)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}

            s.is_paid = 1;
           await studentService.UpdatePayment(s);
            ViewBag.msg = "Payment Accepted Successfully";
            ModelState.Clear();
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<RegistrationModel> lst =await studentService.GetAllRegistrations(emp.branch_id);
            StudentPaymentModel sm = new StudentPaymentModel();
            //ViewBag.registrations = new SelectList(lst, "registration_id", "student_name");
            ViewBag.registrations = await GetRegisteredStudents();

            return View(sm);
        }

        public async Task<List<SelectListItem>> GetRegisteredStudents()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<SelectListItem> lst = new List<SelectListItem>();
            foreach(RegistrationModel s in await studentService.GetAllRegistrations(emp.branch_id))
            {
                string name = s.student_name + " " + s.last_name + "("+s.course_name+")";
                SelectListItem sm = new SelectListItem() { Value=s.registration_id.ToString(), Text=name };
                lst.Add(sm);
            }
            return lst;
        }
        public async Task<JsonResult> GetStudentDetails()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);


            List<StudentModel> lst =await studentService.GetStudents(emp.branch_id);
            return Json(lst);
        }
        public async Task<JsonResult> GetRegistrationDetails(int id)
        {
            RegistrationModel r=await studentService.GetRegistration(id);
            return Json(r);
        }
        public async Task<JsonResult> GetNextPaymentDetails(int id)
        {
            StudentPaymentModel r =await studentService.GetStudentsNextPaymentDetails(id);
            return Json(r);
        }

        public async Task<IActionResult> Invoices()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            EmployeeModel emp = await employeeService.GetEmployeeByUserId(userId);

            List<StudentPaymentModel> invoices =await studentService.GetStudentPayments(emp.branch_id);
            return View(invoices);
        }
        public async Task<IActionResult> ViewInvoice(int registration_id,int payment_id)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            StudentPaymentModel p =await studentService.GetStudentPayment(payment_id);
            RegistrationModel r=await studentService.GetRegistration(registration_id);
            ViewData["registration"] = r;
            ViewData["payments"] =await studentService.GetStudentWisePreviousPayments(registration_id,payment_id);
            ViewData["student"] =await studentService.GetStudent(r.student_id);
            return View(p);
        }

    }
}
