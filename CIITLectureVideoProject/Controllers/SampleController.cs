using CIITLectureVideoProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CIITLectureVideoProject.Controllers
{
    public class SampleController : Controller
    {
        CIITDbContext db;
        public SampleController(CIITDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(IFormFile videoFile)
        //{
        //    if (videoFile != null && videoFile.Length > 0)
        //    {
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await videoFile.CopyToAsync(memoryStream);
        //            byte[] videoData = memoryStream.ToArray(); // This is your video in binary

        //            // Save videoData to your Database (e.g., via Entity Framework Core)
        //        }
        //    }
        //    ViewBag.msg = "Video uploaded successfully";
        //    return View();
        //}
        [HttpPost]
        [RequestSizeLimit(5000L * 1024L * 1024L * 1024L)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 5000L * 1024L * 1024L * 1024L)]
        public async Task<IActionResult> Index(IFormFile videoFile, string title)
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await videoFile.CopyToAsync(ms);
                    var video = new VideoContent
                    {
                        Title = title,
                        Data = ms.ToArray(), // Convert file to binary
                        ContentType = videoFile.ContentType
                    };

                    db.TblVideoContents.Add(video);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
