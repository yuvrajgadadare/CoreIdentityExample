using ERPSystem_Models;
using ERP_Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ERP_Models;

namespace CoreIdentityExample.Controllers
{
    public class VideoTestController : Controller
    {
        IWebHostEnvironment env;
        IMasterService masterService;
        ITopicService topicService;
        IContentService contentService;
        public VideoTestController(IWebHostEnvironment env,IMasterService masterService, ITopicService topicService, IContentService contentService)
        {
            this.env = env;
            this.masterService = masterService;
            this.topicService = topicService;
            this.contentService = contentService;
        }
        public async Task< IActionResult> Index()
        {
            //GetDriveService("377041377452-jutgjpp8v1ofn7ecroj483psq4begvlh.apps.googleusercontent.com", "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");
            //UploadFilesToDrive();
         //   List<VideoModel> lst = await GetVideos();
            ViewBag.folder = "https://drive.google.com/file/d";
            string reg = HttpContext.Session.GetString("registration");
            RegistrationModel r = (RegistrationModel)JsonConvert.DeserializeObject<RegistrationModel>(reg);
            List<TopicModel> topics = await topicService.GetTopicByCourseIds(r.course_id);
            ViewData["topics"] = topics;
            List<TopicModel> lst = await topicService.GetAllYoutubeTopicVideos(topics);

            return View(lst);
        }
        public async Task<IActionResult> TopicWiseVideos(int id)
        {
            TopicModel topic = await contentService.GetTopicWiseContentVideos(id);
            ViewBag.folder = "https://drive.google.com/file/d";
            string reg = HttpContext.Session.GetString("registration");
            RegistrationModel r = (RegistrationModel)JsonConvert.DeserializeObject<RegistrationModel>(reg);
            List<TopicModel> topics = await topicService.GetTopicByCourseIds(r.course_id);
            ViewData["topics"] = topics;
            List<TopicModel> lst = await topicService.GetAllYoutubeTopicVideos(topics);
            return View(topic);
        }
        public async Task<IActionResult> Play(string id,int topic_id, string title)
        {
            ViewBag.folder = "https://drive.google.com/file/d";
            ViewBag.video_file_id = id;
            ViewBag.video_title = title;
            TopicModel topic = await contentService.GetTopicWiseContentVideos(topic_id);
            string reg = HttpContext.Session.GetString("registration");
            RegistrationModel r = (RegistrationModel)JsonConvert.DeserializeObject<RegistrationModel>(reg);
            List<TopicModel> topics = await topicService.GetTopicByCourseIds(r.course_id);
            ViewData["topics"] = topics;
            List<TopicModel> lst = await topicService.GetAllYoutubeTopicVideos(topics);
            return View(topic);

        }
        private DriveService GetDriveService(string clientId, string clientSecret)
        {
            string[] scopes = new string[] { DriveService.Scope.DriveReadonly }; // Read-only access
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret },
                scopes,
                Environment.UserName,
                CancellationToken.None,
                new FileDataStore("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") // Stores tokens in a file
            ).Result;

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "CoreWebApplicationDriveAccess",
            });
        }
        public void UploadFilesToDrive()
        {
            string credentialsPath =env.WebRootPath+"/secret/client_secret.json";
            string folderId = "1t2DHyXfJe4en_lop0jd_nQhzHn5yGBfj";
            string fileToUpload = "D\\MyFile.txt";

            GoogleCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
                {
                    DriveService.ScopeConstants.DriveFile
                });
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Excel Export Upload Console App"
                });

                var fileMetaData = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = Path.GetFileName(fileToUpload),
                    Parents = new List<string> { folderId }
                };

                FilesResource.CreateMediaUpload request;
                using (var streamFile = new FileStream(fileToUpload, FileMode.Open))
                {
                    request = service.Files.Create(fileMetaData, streamFile, "");
                    request.Fields = "id";
                    request.Upload();
                }
                var uploadedFile = request.ResponseBody;
                Console.WriteLine($"File '{fileMetaData.Name}' uploaded with ID: {uploadedFile.Id}");
            }
        }
        public async Task< List<VideoModel>> GetVideos()
        {
            List<VideoModel> lst = new List<VideoModel>();

            var httpClient = new HttpClient();
            var publicFolderId = "19uvwt8_anE869Zbzu2gFDWQvNyUGhtE3";
            var googleDriveApiKey = "AIzaSyDIaGnTInCqBeUCJXEqO5ignIyVk9YLNzE";
            var nextPageToken = "";
            do
            {
                var folderContentsUri = $"https://www.googleapis.com/drive/v3/files?q='{publicFolderId}'+in+parents&key={googleDriveApiKey}";

                if (!String.IsNullOrEmpty(nextPageToken))
                {
                    folderContentsUri += $"&pageToken={nextPageToken}";
                }
                ViewBag.folder = "https://drive.google.com/file/d";
                var contentsJson = await httpClient.GetStringAsync(folderContentsUri);
                var contents = (JObject)JsonConvert.DeserializeObject(contentsJson);
                nextPageToken = (string)contents["nextPageToken"];
                foreach (var file in (JArray)contents["files"])
                {
                    var id = (string)file["id"];
                    var name = (string)file["name"];
                    Console.WriteLine($"{id}:{name}");
                    lst.Add(new VideoModel { video_file_id = id, video_title = name.Split('.')[0] });
                }
            } while (!String.IsNullOrEmpty(nextPageToken));

            return lst;
        }
    }
}
