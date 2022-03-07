using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumTechTest
{
    public class InfiniteScroll
    {
        IWebDriver page;
        string scrollURL = "http://the-internet.herokuapp.com/infinite_scroll";

        [SetUp]
        public void SetUpChromeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            page = new ChromeDriver();

            page.Url = scrollURL;
            Assert.AreEqual(page.Url, scrollURL);
            page.Manage().Window.Maximize();
        }

        private string ScrollBy(int xPosition = 0, int yPosition = 0)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)page;
            string script = string.Format("window.scrollBy({0}, {1})", xPosition, yPosition);
            TestContext.WriteLine("Using Script: " + script);
            string scroll = (string)js.ExecuteScript(script);
            return scroll;
        }

        [Test]
        public void ScrollDownAndUp()
        {
            // Scroll Down By 4000px Twice
            ScrollBy(0, 4000);
            ScrollBy(0, 4000);
                //Thread.Sleep(2000); Wait for 2s to ensure the page has moved

            // Scroll To Top
            IJavaScriptExecutor js = (IJavaScriptExecutor)page;
            string toTop = (string)js.ExecuteScript("window.scroll(0, 0)");

            string heading = page.FindElement(By.XPath("//h3")).Text;
            Assert.AreEqual(heading, "Infinite Scroll");
        }
    }
}
