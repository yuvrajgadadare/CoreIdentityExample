using ERP_Models;
using ERP_Services.Interfaces;
using ERPSystem_Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        public async Task<IActionResult> Videos(int id)
        {
            BatchPlayListModel playlist = null;
            
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = _apiKey,
                    ApplicationName = "MyYoutubeProject"
                });
                // List<PlaylistModel> lst = new List<PlaylistModel>();
                BatchPlayListModel p = await playlistService.GetBatchWisePlayList(id);
                  playlist = new BatchPlayListModel { playlist_key = p.playlist_key, playlist_title = p.playlist_title };
                // 2. Get Videos for each Playlist (simplified loop)
                string nextToken = "";
                while (nextToken != null)
                {
                    var vRequest = youtubeService.PlaylistItems.List("snippet");
                    vRequest.PlaylistId = playlist.playlist_key;
                    vRequest.PageToken = nextToken;
                    var vResponse = await vRequest.ExecuteAsync();

                    playlist.Videos.AddRange(vResponse.Items.Select(v => new VideoModel
                    {
                        VideoId = v.Snippet.ResourceId.VideoId,
                        Title = v.Snippet.Title,
                        ThumbnailUrl = v.Snippet.Thumbnails.Medium?.Url
                    }));
                    nextToken = vResponse.NextPageToken;

                    //playlist.Videos.Add(playlist);
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
