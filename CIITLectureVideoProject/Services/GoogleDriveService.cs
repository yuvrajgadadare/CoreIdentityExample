using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;

namespace CIITLectureVideoProject.Services
{
    public class GoogleDriveService
    {

        private readonly IWebHostEnvironment _env;
        ICacheService cacheService;
        public GoogleDriveService(IWebHostEnvironment env,ICacheService cacheService)
        {
            _env = env;
            this.cacheService = cacheService;
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
            var cacheData = cacheService.GetData<IList<Google.Apis.Drive.v3.Data.File>>("Folders");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var service = GetService();

            var request = service.Files.List();

            request.Q = $"'{parentId}' in parents and mimeType='application/vnd.google-apps.folder' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            cacheData = request.Execute().Files;
            cacheService.SetData<IList<Google.Apis.Drive.v3.Data.File>>("Folders", cacheData, expirationTime);
            return cacheData;
        }



        public IList<Google.Apis.Drive.v3.Data.File>  GetRootVideos(string parentId)
        {

            var cacheData = cacheService.GetData<IList<Google.Apis.Drive.v3.Data.File>>("RootVideos");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var service = GetService();

            var request = service.Files.List();

            request.Q =
        $"'{parentId}' in parents and mimeType contains 'video' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            cacheData = request.Execute().Files;
            cacheService.SetData<IList<Google.Apis.Drive.v3.Data.File>>("RootVideos", cacheData, expirationTime);
            return cacheData;
        }



        public IList<Google.Apis.Drive.v3.Data.File>  GetVideosByFolder(string folderId)
        {
            var cacheData = cacheService.GetData<IList<Google.Apis.Drive.v3.Data.File>>("VideosByFolder");
            if (cacheData != null)
            {
                return cacheData;
            }
            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
            var service = GetService();

            var request = service.Files.List();

            request.Q =  $"'{folderId}' in parents and mimeType contains 'video' and trashed=false";

            request.Fields = "files(id,name,mimeType)";

            cacheData = request.Execute().Files;
            cacheService.SetData<IList<Google.Apis.Drive.v3.Data.File>>("VideosByFolder", cacheData, expirationTime);
            return cacheData;
        }

    }
}
