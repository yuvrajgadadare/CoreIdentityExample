using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace CIITLectureVideoProject.Services
{
    public class GoogleDriveService
    {

        private readonly IWebHostEnvironment _env;

        public GoogleDriveService(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Obsolete]
        public DriveService GetService()
        {
            var path = Path.Combine(_env.ContentRootPath, "creadentials.json");

            var credential = GoogleCredential
                .FromFile(path)
                .CreateScoped(DriveService.Scope.DriveReadonly);

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "YSAAS"
            });
        }


        public IList<Google.Apis.Drive.v3.Data.File>  GetFolders(string parentId)
        {
            var service = GetService();

            var request = service.Files.List();

            request.Q = $"'{parentId}' in parents and mimeType='application/vnd.google-apps.folder' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            return request.Execute().Files;
        }



        public IList<Google.Apis.Drive.v3.Data.File>
        GetRootVideos(string parentId)
        {
            var service = GetService();

            var request = service.Files.List();

            request.Q =
        $"'{parentId}' in parents and mimeType contains 'video' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            return request.Execute().Files;
        }



        public IList<Google.Apis.Drive.v3.Data.File>  GetVideosByFolder(string folderId)
        {
            var service = GetService();

            var request = service.Files.List();

            request.Q =
        $"'{folderId}' in parents and mimeType contains 'video' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            return request.Execute().Files;
        }

    }
}
