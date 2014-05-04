using System;

public class Albums
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string AlbumName { get; set; }
    public string DownloadURL { get; set; }
    public string ViewURL
    {
        get
        {
            return ThumbnailLink.Replace("=s220", "=s4000");
        }
    }
    public string ThumbnailLink { get; set; }
}