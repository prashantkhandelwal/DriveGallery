using System;
using System.Collections.Generic;
using System.Web;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Logging;
using Google.Apis.Services;
using System.Threading.Tasks;
using System.Threading;
//using Google.Apis.Upload;

public class GoogleDrive
{
    DriveService service = null;
    public GoogleDrive()
    {
        Run().Wait();
    }


    private File GetFileByID(string fileID, DriveService service)
    {
        File file = service.Files.Get(fileID).Execute();
        if (file.ExplicitlyTrashed == null)
            return file;
        return null;
    }

    public string GetFolderID(string FolderName)
    {
        FilesResource.ListRequest request = service.Files.List();
        request.Q = "title = '" + FolderName + "'";
        FileList files = request.Execute();
        return files.Items[0].Id;
    }
    public List<File> ListFolderContent(string FolderID)
    {
        ChildrenResource.ListRequest request = service.Children.List(FolderID);
        List<File> files = new List<File>();
        //request.Q = "mimeType = 'image/jpeg'";

        request.Q = "'" + FolderID + "' in parents";

        do
        {
            try
            {
                ChildList children = request.Execute();

                foreach (ChildReference child in children.Items)
                {
                    files.Add(GetFileByID(child.Id, service));
                }

                request.PageToken = children.NextPageToken;

            }
            catch (Exception e)
            {
                request.PageToken = null;
            }
        } while (!String.IsNullOrEmpty(request.PageToken));
        return files;
    }

    public List<File> ListRootFileFolders()
    {
        List<File> result = new List<File>();
        FilesResource.ListRequest request = service.Files.List();
        do
        {
            try
            {
                FileList files = request.Execute();

                result.AddRange(files.Items);
                request.PageToken = files.NextPageToken;
            }
            catch (Exception e)
            {
                request.PageToken = null;
            }
        } while (!String.IsNullOrEmpty(request.PageToken));
        return result;
    }

    public byte[] DownloadFile(string fileID)
    {
        File file = service.Files.Get(fileID).Execute();
        var bytes = service.HttpClient.GetByteArrayAsync(file.DownloadUrl);
        return  bytes.Result;
    }


    private async Task Run()
    {
        try
        {
            GoogleWebAuthorizationBroker.Folder = "Files";
            UserCredential credential;
            using (var stream = new System.IO.FileStream("client_secrets.json",
                System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, DriveConstants.Scopes, "user", CancellationToken.None);
            }

            var drvService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Sample",

            });

            if (drvService != null)
            {
                service = drvService;
            }
        }
        catch { }
    }
}
