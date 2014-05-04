using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biggy;
using Biggy.JSON;

public class Storage : Albums
{
    IBiggyStore<Albums> albumStore = null;
    IBiggyStore<Settings> settingStore = null;
    List<string> AlbumList = new List<string>();

    public Storage()
    {
        settingStore = new JsonStore<Settings>(dbPath: HttpRuntime.AppDomainAppPath);
    }
    public Storage(string Albumname)
    {
        settingStore = new JsonStore<Settings>(dbPath: HttpRuntime.AppDomainAppPath);
        albumStore = new JsonStore<Albums>(dbPath: HttpRuntime.AppDomainAppPath);
        AlbumList.Add(Albumname);
    }
    //public Storage(string[] Albumname)
    //{
    //    settingStore = new JsonStore<Settings>(dbPath: HttpRuntime.AppDomainAppPath);
    //    albumStore = new JsonStore<Albums>(dbPath: HttpRuntime.AppDomainAppPath);
    //    AlbumList.AddRange(Albumname);
    //}
    void Refresh()
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
        ResetAlbum();
        Refresh();
        return albumStore.Load().FindAll(x => x.AlbumName == Albumname);
    }

    public void UpdateAlbums()
    {
        for (int i = 0; i < AlbumList.Count; i++)
        {
            var store = new BiggyList<Settings>(settingStore);
            Settings setting = new Settings();
            setting.AlbumName = AlbumList[i];
            store.Add(setting);
        }
    }

    public List<string> GetAllAlbums()
    {
        List<string> str = new List<string>();
        if (settingStore.Load().Count > 0)
        {
            foreach (var item in settingStore.Load().GroupBy(x => x.AlbumName).Select(y => y.First()).ToList())
            {
                str.Add(item.AlbumName);
            }
        }
       
        return str;
    }

    public void ResetSettings()
    {
        if (settingStore.Load() != null)
            settingStore.Clear();
    }
    void ResetAlbum()
    {
        if (albumStore.Load() != null)
            albumStore.Clear();
    }
}