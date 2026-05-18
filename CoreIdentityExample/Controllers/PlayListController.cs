using ERP_Models;
using ERP_Services.Interfaces;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CoreIdentityExample.Controllers
{
    public class PlayListController : Controller
    {
        IPlaylistService playlistService;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey = "AIzaSyBsWpL3OGd64qfAyD-33Vv9OuH6Gq1eVXg";
        public PlayListController(IPlaylistService playlistService, IMemoryCache cache)
        {
            this.playlistService = playlistService;
            _cache = cache;

        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(PlaylistModel c)
        {
            playlistService.AddPlayList(c);
            ModelState.Clear();
            ViewBag.msg = "PlayList Added Successfully";
            return View();
        }


        public async Task<IActionResult> AllPlayLists()
        {
            List<PlaylistModel> lst = await playlistService.GetPlayLists();
            return View(lst);

        }

        public async Task<IActionResult> ViewPlayList(int id)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _apiKey,
                ApplicationName = "MyYoutubeProject"
            });
            // List<PlaylistModel> lst = new List<PlaylistModel>();
            PlaylistModel p = await playlistService.GetPlayList(id);
            var playlist = new PlaylistModel { PlayListKey = p.PlayListKey, PlayListTitle = p.PlayListTitle };
            // 2. Get Videos for each Playlist (simplified loop)
            string nextToken = "";
            while (nextToken != null)
            {
                var vRequest = youtubeService.PlaylistItems.List("snippet");
                vRequest.PlaylistId = playlist.PlayListKey;
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

            List<PlaylistModel> lst = await playlistService.GetPlayLists();
            ViewData["playlists"] = lst;
            return View(playlist);
        }
    }
}
