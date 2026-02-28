using CIITLectureVideoProject.Models;
using CIITLectureVideoProject.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CIITLectureVideoProject.Controllers
{
    //[Authorize]
    public class VideoController : Controller
    {
        private readonly GoogleDriveService _driveService;
        private readonly IConfiguration _config;

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
             var parentId =_config["GoogleDrive:FolderId"];

            var model = new DriveIndexViewModel
            {
                Folders = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetFolders(parentId),

                Videos = (List<Google.Apis.Drive.v3.Data.File>)_driveService.GetRootVideos(parentId)
            };
            ViewData["videodata"] = model;
            ViewBag.VideoId = id;
            return View();
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
    }
}
