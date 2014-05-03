using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biggy;
using Biggy.JSON;

public class Albums
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string AlbumName { get; set; }
    public string DownloadURL { get; set; }

    //public string ViewURL
    //{
    //    get
    //    {
    //        return DownloadURL.Replace("&export=download", string.Empty);
    //    }
    //}
    public string ThumbnailLink { get; set; }
    
}