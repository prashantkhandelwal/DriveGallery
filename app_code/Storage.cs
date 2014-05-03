using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biggy;
using Biggy.JSON;

public class Storage : Albums
{
    IBiggyStore<Albums> albumStore = null;
    List<string> AlbumList = new List<string>();

    public Storage() { albumStore = new JsonStore<Albums>(dbPath: HttpRuntime.AppDomainAppPath); }
    public Storage(string[] Albumname)
    {
        albumStore = new JsonStore<Albums>(dbPath: HttpRuntime.AppDomainAppPath);
        AlbumList.AddRange(Albumname);
    }
    public void Refresh()
    {
        var store = new BiggyList<Albums>(albumStore);
        List<Albums> albumContent = new List<Albums>();
        var drive = new GoogleDrive();


        if (AlbumList.Count > 0)
        {
            for (int i = 0; i < AlbumList.Count; i++)
            {
                string AlbumId = drive.GetFolderID(AlbumList[i]);
                var contents = drive.ListFolderContent(AlbumId);
                foreach (var image in contents)
                {
                    Albums album = new Albums();
                    album.Id = AlbumId;
                    album.Title = image.Title;
                    album.AlbumName = AlbumList[i];
                    album.ThumbnailLink = image.ThumbnailLink;
                    album.DownloadURL = image.WebContentLink;

                    albumContent.Add(album);
                }
            }
            store.Add(albumContent);
        }
    }

    public List<Albums> GetAlbumDetails(string Albumname)
    {
        return albumStore.Load().FindAll(x => x.AlbumName == Albumname);
    }

    public List<string> GetAllAlbums()
    {
        List<string> str = new List<string>();
        var l = albumStore.Load().GroupBy(x => x.AlbumName).Select(y => y.First()).ToList();
        foreach (var item in l)
        {
            str.Add(item.AlbumName);
        }
        return str;
    }

    public void Clear()
    {
        if (albumStore.Load() != null)
            albumStore.Clear();
    }
}