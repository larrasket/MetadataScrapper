using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using static System.String;

namespace MetadataScrapper;

public class MetaFacebook : IDisposable
{
    private static readonly IWebDriver Driver;
    private static readonly HtmlDocument Doc;
    private static Actions? _action;

    static MetaFacebook()
    {
        Driver = new ChromeDriver();
        Doc = new HtmlDocument();
    }

    private static void Navigate(string url)
    {
        if (Driver.Url == url) return;
        Driver.Navigate().GoToUrl(url);
        _action = new Actions(Driver);
    }

    private static int ExtractCount(string s) => int.Parse(Regex.Match(s, @"\d+").Value);

    public static int PageLikes(string url)
    {
        Navigate(url);
        Thread.Sleep(2000);
        var l = Driver.PageSource.IndexOf("people</span>", StringComparison.CurrentCulture);
        var i = l;
        while (Driver.PageSource[i] != '>') i--;
        var rss = Driver.PageSource.Substring(i, l - i + 1).Replace(",", Empty);
        return ExtractCount(rss);
    }

    private static int FromPost(string url, string span)
    {
        Navigate(url);
        var we = Driver.FindElements(By.XPath($"//span[@class='{span}']")).LastOrDefault();
        if (we == null) return -1;
        var checker = we.Text.Replace(" ", Empty).Replace("comments", Empty)
            .Replace("shares", Empty);

        if (checker[^1].ToString() != "K")
            return int.Parse(checker);
#pragma warning disable CS8602
        _action.MoveToElement(we).MoveToElement(we).Build().Perform();
#pragma warning restore CS8602
        Thread.Sleep(2000);
        Doc.LoadHtml(Driver.PageSource);
        var nodes = Doc.DocumentNode.SelectSingleNode("//div[@class='tooltipContent']");
        var t = nodes.InnerText.Replace(",", Empty);
        return ExtractCount(t) + 19;
    }


    public static int PostLikes(string url) => FromPost(url, Spans.Facebook.Likes);

    public static int PostComments(string url) => FromPost(url, Spans.Facebook.Comments);

    public static int PostShares(string url) => FromPost(url, Spans.Facebook.Shares);

    public void Dispose()
    {
        Driver.Close();
        Driver.Dispose();
    }

    public static void Close()
    {
        Driver.Close();
        Driver.Dispose();
    }
}

public static class Spans
{
    public static class Facebook
    {
        public static string Likes = "_81hb";
        public static string Comments = "_1whp _4vn2";
        public static string Shares = "_355t _4vn2";
    }
}