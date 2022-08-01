// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using HtmlAgilityPack;
using MetadataScrapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Interactions;

//var pageurl =
//    "https://www.facebook.com/facebook";
//var posturl =
//    "https://www.facebook.com/zuck/posts/pfbid02EuNrFgScgg8BzUeGtvFg1RZRGE4i6oskK2TCSkoAzYuJgekE3xs6AKJqtvkGxLn3l";


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




ChromeDriver driver;
var chromeDriverService = ChromeDriverService.CreateDefaultService(@"C:\Users\Delta Store\Downloads");
ChromeOptions options = new ChromeOptions();
options.AddUserProfilePreference(CapabilityType.EnableProfiling, true);
options.SetLoggingPreference("performance", LogLevel.Severe);
options.AddArgument("--disable-gpu");
options.AddArgument("disable-infobars");
options.AddArgument("--no-sandbox");
options.AddArguments("--disable-storage-reset");
options.AddArguments("--no-first-run");
driver = new ChromeDriver(chromeDriverService, options);
driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(70);
driver.Manage().Window.Maximize();

IDevToolsSession _devTools;
DevToolsSessionDomains _devToolsSession;
Console.WriteLine("Chrome Performance Metrics");
var logs = driver.Manage().Logs.GetLog("performance");
for (int i = 0; i < logs.Count; i++)
{
    Debug.WriteLine(logs[i].Message);
}
_devTools = driver.GetDevToolsSession();
_devToolsSession = _devTools.GetVersionSpecificDomains<DevToolsSessionDomains>();
var posturl = "https://www.instagram.com/instagram/";
driver.Navigate().GoToUrl(posturl);
HtmlDocument doc = new HtmlDocument();
Actions action = new Actions(driver);
IWebElement we = driver.FindElements(By.XPath("//body")).Last();
action.MoveToElement(we).MoveToElement(we).Build().Perform();
Thread.Sleep(2000);
doc.LoadHtml(driver.PageSource);
using (var client = new HttpClient())
{
    var response = await client.GetAsync(posturl);
    if (response.IsSuccessStatusCode)
    {
        var htmlBody = await response.Content.ReadAsStringAsync();
        //Console.WriteLine(htmlBody);
        doc.LoadHtml(htmlBody);

        var uselessString = "window._sharedData = ";
        var scripts = doc.DocumentNode.SelectNodes("/html/body/script");
        var scriptInnerText = scripts[0].InnerText.Substring(uselessString.Length).Replace(";", " ");
        Console.WriteLine(scriptInnerText);
        dynamic jsonStaff = JObject.Parse(scriptInnerText);
        dynamic userProfile = jsonStaff["entry_data"]["ProfilePage"][0]["graphql"]["user"];
        dynamic userLike = jsonStaff["entry_data"]["ProfilePage"]["edge_followed_by"]["count"];
        Console.WriteLine(userLike);
        await System.IO.File.WriteAllTextAsync("user.json", JsonConvert.SerializeObject(userProfile), Encoding.UTF8);
        var InstagramUser = new InstagramUser
        {
            FullName = userProfile.full_name,
            FollowerCount = userProfile.edge_followed_by.Count,
            FolloweingCount = userProfile.edge_follow.Count,
        };
        InstagramUser.Display();
        driver.Quit();
    }


}

