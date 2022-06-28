using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using static System.String;

namespace MetadataScrapper;

public class MetaBase
{
    protected readonly IWebDriver Driver;
    protected readonly HtmlDocument Doc;
    protected Actions Action;

    protected void Navigate(string url)
    {
        //TODO Clear Cookies
        if (Driver.Url == url) return;
        Driver.Navigate().GoToUrl(url);
        Action = new Actions(Driver);
    }

#pragma warning disable CS8618
    public MetaBase()
#pragma warning restore CS8618
    {
        Driver = new ChromeDriver();
        Doc = new HtmlDocument();
    }

    protected static int ExtractCount(string s) => int.Parse(Regex.Match(s, @"\d+").Value);
}

public class MetaFacebook : MetaBase, IMetaCollector
{
    public Task<int> Likes(string url)
    {
        Navigate(url);
        Thread.Sleep(2000);
        var l = Driver.PageSource.IndexOf("people</span>", StringComparison.CurrentCulture);
        var i = l;
        while (Driver.PageSource[i] != '>') i--;
        var rss = Driver.PageSource.Substring(i, l - i + 1).Replace(",", Empty);
        return Task.FromResult(ExtractCount(rss));
    }

    private Task<int> ObjectMetaScrapper(string url, string span)
    {
        Navigate(url);
        var we = Driver.FindElements(By.XPath($"//span[@class='{span}']")).LastOrDefault();
        if (we == null) return Task.FromResult(-1);
        var checker = we.Text.Replace(" ", Empty).Replace("comments", Empty)
            .Replace("shares", Empty);

        if (checker[^1].ToString() != "K")
            return Task.FromResult(int.Parse(checker));
#pragma warning disable CS8602
        Action.MoveToElement(we).MoveToElement(we).Build().Perform();
#pragma warning restore CS8602
        Thread.Sleep(2000);
        Doc.LoadHtml(Driver.PageSource);
        var nodes = Doc.DocumentNode.SelectSingleNode("//div[@class='tooltipContent']");
        var t = nodes.InnerText.Replace(",", Empty);
        return Task.FromResult(ExtractCount(t) + 19);
    }

    public Task<int> PostLikes(string url) => ObjectMetaScrapper(url, Spans.Facebook.Likes);

    public Task<int> Comments(string url) => ObjectMetaScrapper(url, Spans.Facebook.Comments);

    public Task<int> Shares(string url) => ObjectMetaScrapper(url, Spans.Facebook.Shares);

    public void Close()
    {
        Driver.Close();
        Driver.Dispose();
    }
}
