using ERP_Models;
using ERP_Services.Interfaces;
using ERPSystem_Models;
 
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoreIdentityExample.Areas.BatchManagement.Controllers
{
    [Area("BatchManagement")]

    public class ProfileController : Controller
    {
        IEmployeeService employeeService;
        private IWebHostEnvironment environment;

        public ProfileController(IEmployeeService employeeService, IWebHostEnvironment environment)
        {
            this.employeeService = employeeService;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //string employee = HttpContext.Session.GetString("employee");
            //EmployeeModel tr = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(employee);
            //// ViewData["trainer"] = tr;
            string user_name = User.Identity.Name;
            ///
            ViewBag.user = user_name;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmployeeModel employee)
        {
            //if (HttpContext.Session.GetString("employee") == null)
            //{
            //    return Redirect("/Account/Login");
            //}
            //employeeService.UpdateEmployeeDetails(employee);
            //ViewBag.msg = "Employee details are updated successfully";
            //EmployeeModel emp =await employeeService.GetEmployee(employee.employee_id);
            //HttpContext.Session.SetString("employee", JsonConvert.SerializeObject(emp));
            //HttpContext.Session.SetString("employee_name", emp.employee_name);
            //HttpContext.Session.SetInt32("employee_id", emp.employee_id);
            //string trainer = HttpContext.Session.GetString("employee");
            string user_name = User.Identity.Name;
            ViewBag.user = user_name;

            return View();
        }
        //public async Task<string> ChangePassword(ChangePasswordModel model)
        //{

        //    string trainer = HttpContext.Session.GetString("employee");
        //    EmployeeModel tr = (EmployeeModel)JsonConvert.DeserializeObject<EmployeeModel>(trainer);
        //    EmployeeModel emp =await employeeService.CheckEmployeeLogin(tr.employee_code, model.current_password);
        //    if (emp == null)
        //    {
        //        return "Current password is not matched";

        //    }
        //    else if (model.new_password != model.re_password)
        //    {
        //        return "Current password  and Confirm password are not matched";

        //    }
        //    else
        //    {
        //        EmployeeModel em = new EmployeeModel() { employee_id = tr.employee_id, password = model.new_password };
        //        employeeService.ChangeEmployeeDetailPassword(em);
        //        return "1";
        //    }
        //}

        public async Task<string> ChangeProfilePhoto(IFormFile file)
        {
            int employee_id = (int)HttpContext.Session.GetInt32("employee_id");
            EmployeeModel d =await employeeService.GetEmployee(employee_id);
            Random r = new Random();
            int n = r.Next(1, 1000);
            string imgname = d.employee_name + "_" + n + Path.GetExtension(file.FileName);
            string imgpath = environment.WebRootPath + "/Employees/" + imgname;
            if (System.IO.File.Exists(imgpath))
            {
                System.IO.File.Delete(imgpath);
            }
            FileStream fs = new FileStream(imgpath, FileMode.Create, FileAccess.Write);
            file.CopyTo(fs);
            // d.profile_photo = imgname;
            EmployeeModel emp = new EmployeeModel() { employee_id = employee_id, profile_photo = imgname };
           await employeeService.ChangeProfilePhoto(emp);
            //string aadharname = d.student_name + "_adhr_" + r.Next(1, 1000) + Path.GetExtension(aadharcard.FileName);
            //string aadharpath = environment.WebRootPath + "/Students/AadharCards/" + aadharname;
            //if (System.IO.File.Exists(aadharpath))
            //{
            //    System.IO.File.Delete(aadharpath);
            //}
            //FileStream fsaadhar = new FileStream(aadharpath, FileMode.Create, FileAccess.Write);
            //aadharcard.CopyTo(fsaadhar);
            //d.aadhar_card_photo = aadharname;
            d = await employeeService.GetEmployee(employee_id);
            HttpContext.Session.SetString("employee", JsonConvert.SerializeObject(d));
            HttpContext.Session.SetString("employee_name", d.employee_name);
            HttpContext.Session.SetInt32("employee_id", d.employee_id);
            return "Profile Photo Changed Successfully";

        }
    }
}
