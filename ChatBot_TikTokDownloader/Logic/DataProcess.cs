using System.Net;
using HtmlAgilityPack;

namespace ChatBot_TikTokDownloader;

public class DataProcess
{
    private const string MarkWord = "playAddr";
    private const string TikTok = "tiktok";

    public static bool HtmlAgilityPackParse(string url, string fileName)
    {
        if (!url.Contains(TikTok)) return false;
        var web = new HtmlWeb();
        HtmlDocument doc;
        try
        {
            doc = web.Load(url);
        }
        catch
        {
            return false;
        }
        var data = doc.DocumentNode.OuterHtml.Replace("u002F", "");
        var videoUrl = string.Empty;
        if (!data.Contains(MarkWord)) return false;
        var startNumber = data.IndexOf(MarkWord, StringComparison.Ordinal);
        for (var i = startNumber + 11; i < data.Length; i++)
        {
            if (data[i] != '"')
            {
                videoUrl += data[i].ToString();
            }
            else
            {
                break;
            }
        }
        var replace = videoUrl.Replace('\\', '/');
        new WebClient().DownloadFile(new Uri(@replace), fileName + ".mp4");
        return true;
    }
}