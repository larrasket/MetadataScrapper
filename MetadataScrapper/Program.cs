// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using System.Threading.Channels;
using HtmlAgilityPack;
using MetadataScrapper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;

var pageurl =
    "https://www.facebook.com/facebook";
var posturl =
    "https://www.facebook.com/zuck/posts/pfbid02EuNrFgScgg8BzUeGtvFg1RZRGE4i6oskK2TCSkoAzYuJgekE3xs6AKJqtvkGxLn3l";

Console.WriteLine("Page Likes:");
Console.WriteLine(MetaFacebook.PageLikes(pageurl));
Console.WriteLine("Post Likes:");
Console.WriteLine(MetaFacebook.PostLikes(posturl));
Console.WriteLine("Post Comments:");
Console.WriteLine(MetaFacebook.PostComments(posturl));
Console.WriteLine("Post Shares:");
Console.WriteLine(MetaFacebook.PostShares(posturl));
MetaFacebook.Close();

// var Driver = new ChromeDriver();
// HtmlDocument doc = new HtmlDocument();
// Driver.Navigate().GoToUrl(posturl);
// Actions action = new Actions(Driver);
// IWebElement we = Driver.FindElements(By.XPath("//span[@class='_1whp _4vn2']")).Last();
// // if (we.Text.Last() == 'K')
// {
//     action.MoveToElement(we).MoveToElement(we).Build().Perform();
//     Thread.Sleep(2000);
//     doc.LoadHtml(Driver.PageSource);
//     var nodes = doc.DocumentNode.SelectSingleNode("//div[@class='tooltipContent']");
//     var t = nodes.InnerText.Replace("comments", String.Empty);
//     Console.WriteLine(t);
//     // Console.WriteLine(int.Parse(Regex.Match(t, @"\d+").Value));
// }


// var uri = "https://www.facebook.com/facebook";
// var Driver = new ChromeDriver();
// Driver.Navigate().GoToUrl(uri);
// Console.WriteLine(Driver.PageSource);