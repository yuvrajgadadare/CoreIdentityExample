//using CoreIdentityExample.Services;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;

namespace CoreIdentityExample.Controllers
{
    public class SampleController : Controller
    {
        //IExtraService extraService;
        //public SampleController(IExtraService extraService)
        //{
        //    this.extraService = extraService;
        //}
        //public IActionResult Index(string file)
        //{
        //    //DataTable dt = extraService.ReadExcelFiles(@"C:\Users\CIIT\OneDrive\Desktop\excel\topicdata.xls");
        //    //DataTable dt = extraService.ReadExcelFiles(file);
        //    //List<string> names = new List<string>();
        //    //foreach (DataRow dr in dt.Rows)
        //    //{
        //    //    names.Add(dr[1].ToString());
        //    //}
        //    //return View(names);
        //    return View();
        //}
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
 
