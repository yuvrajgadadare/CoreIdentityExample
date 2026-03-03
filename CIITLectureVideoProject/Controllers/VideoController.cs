using CIITLectureVideoProject.Models;
using CIITLectureVideoProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace CIITLectureVideoProject.Controllers
{
    //[Authorize]
    public class VideoController : Controller
    {
        private readonly GoogleDriveService _driveService;
        private readonly IConfiguration _config;
        public static string folderId="";
        public VideoController(GoogleDriveService driveService,
                               IConfiguration config)
        {
            _driveService = driveService;
            _config = config;
        }
        public IActionResult Index()
        {
            var parentId =
                _config["GoogleDrive:FolderId"];

            var model = new DriveIndexViewModel
            {
                Folders = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetFolders(parentId),

                Videos = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetRootVideos(parentId)
            };
            return View(model);
        }
        public IActionResult Folder(string id)
        {
            var parentId = _config["GoogleDrive:FolderId"];

            var model = new DriveIndexViewModel
            {
                Folders = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetFolders(parentId),

                Videos = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetRootVideos(parentId)
            };
            ViewData["videodata"] = model;
            var videos =_driveService.GetVideosByFolder(id);
           var folders= _driveService.GetFolders(id);
            folderId = id;
            var model2 = new DriveIndexViewModel
            {
                Folders = (List<Google.Apis.Drive.v3.Data.File>)folders,

                Videos = (List<Google.Apis.Drive.v3.Data.File>)videos
            };
           // ViewData["videodata"] = model2;
            return View(model2);
        }

        public IActionResult Play(string id)
        {
            // var parentId =_config["GoogleDrive:FolderId"];

            //var model = new DriveIndexViewModel
            //{
            //    Folders = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetFolders(parentId),

            //    Videos = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetRootVideos(parentId)
            //};
            //ViewData["videodata"] = model;
            ViewBag.VideoId = id;
            
            var videos = _driveService.GetVideosByFolder(folderId);
            var folders = _driveService.GetFolders(folderId);
          
            var model2 = new DriveIndexViewModel
            {
                Folders = (List<Google.Apis.Drive.v3.Data.File>)folders,

                Videos = (List<Google.Apis.Drive.v3.Data.File>)videos
            };
            return View(model2);
        }

        public async Task<IActionResult> Stream(string id)
        {
            var service = _driveService.GetService();

            var stream = new MemoryStream();

            await service.Files.Get(id)
                .DownloadAsync(stream);

            stream.Position = 0;

            return File(stream, "video/mp4");
        }



        //[HttpGet]
        //public async Task<IActionResult> Stream(string fileId)
        //{
        //    // 1. Initialize the Google Drive Service
        //    DriveService service = _driveService.GetService(); ; // Your helper to get authenticated service

        //    // 2. Prepare the Request
        //    var request = service.Files.Get(fileId);

        //    // 3. Open a stream to Google Drive
        //    // Note: Use a stream that supports seeking if possible, 
        //    // or stream directly to the response.
        //    var stream = new MemoryStream();
        //    await request.DownloadAsync(stream);
        //    stream.Position = 0;

        //    // 4. Return the file with Range Processing enabled
        //    // This allows the browser to request specific byte ranges (Status 206)
        //    return File(stream, "video/mp4", enableRangeProcessing: true);
        //}
    }
}
