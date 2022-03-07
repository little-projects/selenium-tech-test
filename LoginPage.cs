using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome; // Or using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using WebDriverManager.DriverConfigs.Impl;
using System.Web.Security;

namespace SeleniumTechTest
{
    // Test Case 1
    public class LoginPage
    {
        IWebDriver page;
        string loginURL = "http://the-internet.herokuapp.com/login";
        string usernameXPath = "//input[@name='username']";
        string passwordXPath = "//input[@name='password']";
        string submitBtnXPath = "//button[@type='submit']";
        string validUsername = "tomsmith";
        string validPassword = "SuperSecretPassword!";
        string badPasswordErrText = "Your password is invalid!";
        string badUsernameErrText = "Your username is invalid!";

        [SetUp]
        public void EstablishBrowser()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            page = new ChromeDriver();

            //new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
            //page = new FirefoxDriver();

            page.Manage().Window.Maximize();
            page.Url = loginURL;
            //page.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);

            Assert.AreEqual(page.Url, loginURL);

            string pageTitle = page.Title;
            string expectedTitle = "The Internet";
            TestContext.WriteLine("Page Title: " + pageTitle);
            Assert.AreEqual(expectedTitle, pageTitle);
        }

        private string CreateRandomString(int length = 0) //int alphaNumericalChars = 0
        {
            if (length == 0)
            {
                length = new Random().Next(1, 100);
            }

            string randomString = Membership.GeneratePassword(length, 0);
            TestContext.WriteLine("Using Random String: " + randomString);
            return randomString;
        }

        [Test]
        // Scenario 1. A user is unable to login with the correct username & incorrect password.
        public void IncorrectPassword()
        {
            page.FindElement(By.XPath(usernameXPath)).SendKeys(validUsername);
            page.FindElement(By.XPath(passwordXPath)).SendKeys(CreateRandomString());
            page.FindElement(By.XPath(submitBtnXPath)).Click();

            page.FindElement(By.XPath($"//div[@id='flash'][contains(text(), '{badPasswordErrText}')]"));

        }

        [Test]
        // Scenario 1.5. A user is unable to login with the correct username & no password.
        public void MissingPassword()
        {
            page.FindElement(By.XPath(usernameXPath)).SendKeys(validUsername);
            page.FindElement(By.XPath(submitBtnXPath)).Click();
            page.FindElement(By.XPath($"//div[@id='flash'][contains(text(), '{badPasswordErrText}')]"));
        }

        [Test]
        // Scenario 2. A user is unable to login with the incorrect username & correct password.
        public void IncorrectUsername()
        {
            page.FindElement(By.XPath(usernameXPath)).SendKeys(CreateRandomString());
            page.FindElement(By.XPath(passwordXPath)).SendKeys(validPassword);
            page.FindElement(By.XPath(submitBtnXPath)).Click();

            page.FindElement(By.XPath($"//div[@id='flash'][contains(text(), '{badUsernameErrText}')]"));
        }

        [Test]
        // Scenario 3. A user is able to login with the correct username & password.
        //             A user is able to logout following successful login.
        public void SuccessfulLogin()
        {
            page.FindElement(By.XPath(usernameXPath)).SendKeys(validUsername);
            page.FindElement(By.XPath(passwordXPath)).SendKeys(validPassword);

            page.FindElement(By.XPath(submitBtnXPath)).Click();

            // Define the logged in heading & wait for login action to complete
            string loggedInHeading = "Secure Area";

            WebDriverWait wait = new WebDriverWait(page, TimeSpan.FromSeconds(2));
            wait.Until(
                SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(
                    page.FindElement(By.TagName("h2")), loggedInHeading
                )
            );

            string pageHeading = page.FindElement(By.TagName("h2")).Text;
            TestContext.WriteLine("Logged In - Page Heading: " + pageHeading);
            Assert.AreEqual(pageHeading, loggedInHeading);

            string pageContent = page.FindElement(By.TagName("h4")).Text;
            TestContext.WriteLine("Logged In - Page Content: " + pageContent);
            Assert.AreEqual(pageContent, "Welcome to the Secure Area. When you are done click logout below.");

            // FindElement(By.LinkText(" Logout")); vs FindElement(..."//a[@href='/logout']")
            page.FindElement(By.LinkText("Logout")).Click();

            string logoutMsg = "You logged out of the secure area!";
            page.FindElement(By.XPath($"//div[@id='flash'][contains(text(), '{logoutMsg}')]"));
            TestContext.WriteLine("Logged Out - Expected Message: " + logoutMsg);
        }

        [TearDown]
        public void CloseChrome()
        {
            Assert.AreEqual(page.Url, loginURL);
            TestContext.WriteLine("Logged Out - On the /login page");
            page.Close();
        }
    }
}
