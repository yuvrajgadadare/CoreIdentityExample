using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using ERPSystem_Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Options;

namespace CoreIdentityExample.Controllers
{
    public class BatchController : Controller
    {
        IStudentService studentService;
        IMasterService masterService;
        IBatchService batchService;
        private IWebHostEnvironment environment;
        IExtraService extraService;
        EmailSettings _settings;
        IPlaylistService playlistService;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey = "AIzaSyBsWpL3OGd64qfAyD-33Vv9OuH6Gq1eVXg";
        public BatchController(IPlaylistService playlistService, IMemoryCache cache, IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
        {
            this.playlistService = playlistService;
            _cache = cache;
            this.studentService = studentService;
            this.masterService = masterService;
            this.environment = environment;
            this.extraService = extraService;
            _settings = settings.Value;
            this.batchService = batchService;
        }
        //public BatchController(IStudentService studentService, IMasterService masterService, IWebHostEnvironment environment, IExtraService extraService, IOptions<EmailSettings> settings, IBatchService batchService)
        //{
        //    this.studentService = studentService;
        //    this.masterService = masterService;
        //    this.environment = environment;
        //    this.extraService = extraService;
        //    _settings = settings.Value;
        //    this.batchService = batchService;
        //}

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            return View(batches);
        }
        public async Task<IActionResult> AllBatchVideos()
        {
            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            return View(batches);
        }
        public async Task<IActionResult> ViewBatch(int id)
        {
            //int student_id = (int)HttpContext.Session.GetInt32("student_id");
            //List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            //return View(batches.FirstOrDefault(e=>e.batch_id.Equals(id)));

            List<BatchScheduleModel> schedule = await batchService.GetBatchWiseSchedule(id);
            List<BatchStudentModel> students = await batchService.GetBatchWiseStudents(id);
            List<BatchScheduleExamModel> exams = await batchService.GetBatchWiseScheduledExams(id);
            BatchModel b = await batchService.GetBatch(id);

            ViewData["batch"] = b;

            ViewData["students"] = students;
            ViewData["exams"] = exams;
            return View(schedule);
        }
        public async Task<IActionResult> Videos(int id)
        {
            //BatchPlayListModel playlist = null;

            //    var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            //    {
            //        ApiKey = _apiKey,
            //        ApplicationName = "MyYoutubeProject"
            //    });
            //    // List<PlaylistModel> lst = new List<PlaylistModel>();
            //    BatchPlayListModel p = await playlistService.GetBatchWisePlayList(id);
            //      playlist = new BatchPlayListModel { playlist_key = p.playlist_key, playlist_title = p.playlist_title };
            //    // 2. Get Videos for each Playlist (simplified loop)
            //    string nextToken = "";
            //    while (nextToken != null)
            //    {
            //        var vRequest = youtubeService.PlaylistItems.List("snippet");
            //        vRequest.PlaylistId = playlist.playlist_key;
            //        vRequest.PageToken = nextToken;
            //    vRequest.MaxResults = 10;
            //        var vResponse = await vRequest.ExecuteAsync();

            //        playlist.Videos.AddRange(vResponse.Items.Select(v => new VideoModel
            //        {
            //            VideoId = v.Snippet.ResourceId.VideoId,
            //            Title = v.Snippet.Title,
            //            ThumbnailUrl = v.Snippet.Thumbnails.Medium?.Url
            //        }));
            //        nextToken = vResponse.NextPageToken;

            //        //playlist.Videos.Add(playlist);
            //    }

            // 1. Initialize the service instance inside the request scope for thread safety
            //using var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            //{
            //    ApiKey = _apiKey,
            //    ApplicationName = "MyYoutubeProject"
            //});
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "MyYoutubeProject"
            });

            BatchPlayListModel p = await playlistService.GetBatchWisePlayList(id);;
            if (p == null) return null;

            var playlist = new BatchPlayListModel
            {
                playlist_key = p.playlist_key,
                playlist_title = p.playlist_title,
                Videos = new List<VideoModel>()
            };

            string nextToken = "";

            // 2. Loop through pages asynchronously without blocking other system threads
            while (nextToken != null)
            {
                var vRequest = youtubeService.PlaylistItems.List("snippet");
                vRequest.PlaylistId = playlist.playlist_key;
                vRequest.PageToken = string.IsNullOrEmpty(nextToken) ? null : nextToken;
                vRequest.MaxResults = 50; // Optimized for high load by reducing network hops

                // Await ensures this thread is freed back to the pool while waiting for YouTube
                var vResponse = await vRequest.ExecuteAsync();

                if (vResponse?.Items != null)
                {
                    playlist.Videos.AddRange(vResponse.Items.Select(v => new VideoModel
                    {
                        VideoId = v.Snippet.ResourceId?.VideoId,
                        Title = v.Snippet.Title,
                        ThumbnailUrl = v.Snippet.Thumbnails?.Medium?.Url
                    }));
                }

                nextToken = vResponse?.NextPageToken;
            }

         




            if (HttpContext.Session.GetInt32("student_id") == null)
            {
                return RedirectToAction("Login");
            }
            int student_id = (int)HttpContext.Session.GetInt32("student_id");
            List<BatchStudentModel> batches = await batchService.GetStudentWiseBatches(student_id);
            ViewData["playlists"] = batches;
            return View(playlist);
        }
    }
}
