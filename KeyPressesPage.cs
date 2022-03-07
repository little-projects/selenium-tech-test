using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumTechTest
{
    // Test Case 3
    public class KeyPressesPage
    {
        IWebDriver page;
        string keyPressesURL = "http://the-internet.herokuapp.com/key_presses";
        string inputBoxXPath = "//input[@id='target']";
        

        [SetUp]
        public void SetUpChromeDriver()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            page = new ChromeDriver();

            page.Url = keyPressesURL;
            Assert.AreEqual(page.Url, keyPressesURL);
        }

        private string GetResult(string expectedResult) //int alphaNumericalChars = 0
        {
            string resultXPath = $"//p[@id='result'][contains(text(), 'You entered: { expectedResult.ToUpper()}')]";
            TestContext.WriteLine("Looking for XPath: " + resultXPath);

            return resultXPath;
        }

        [Test]
        // Scenario 1. A user enters one random letter or number
        public void RandomAlphanumericCharacter()
        {
            string alphanumerics = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM"; //.ToCharArray();
            char randomAlphanumeric = alphanumerics[new Random().Next(1, alphanumerics.Length)];
            string character = randomAlphanumeric.ToString();

            TestContext.WriteLine("Using Random Letter: " + character);

            page.FindElement(By.XPath(inputBoxXPath)).SendKeys(character);
            page.FindElement(By.XPath(GetResult(character)));
        }
        
        [Test]
        // Scenario 2. A user enters a random word
        public void RandomWord()
        {
            string[] words = { "Well", "Done", "Baby", "Girl" };
            string randomWord = words[new Random().Next(4)];

            for (int i = 0; i < randomWord.Length; i++)
            {
                string individualLetter = randomWord[i].ToString();
                page.FindElement(By.XPath(inputBoxXPath)).SendKeys(individualLetter);
                page.FindElement(By.XPath(GetResult(individualLetter)));
            }
        }


        [Test]
        // Scenario 3. A user enters a random control key
        public void RandomKey()
        {
            // KeyValuePair of key stroke & expected input description
            var list = new List<KeyValuePair<string, string>>();

            list.Add(new KeyValuePair<string, string>(",", "COMMA"));
            list.Add(new KeyValuePair<string, string>(".", "PERIOD"));
            list.Add(new KeyValuePair<string, string>("/", "SLASH"));
            // @wip Is it understood that Shift + '/' recognises the '/'
            list.Add(new KeyValuePair<string, string>("?", "SLASH"));
            list.Add(new KeyValuePair<string, string>(Keys.Shift, "SHIFT"));
            list.Add(new KeyValuePair<string, string>("\\", "BACK_SLASH"));
            list.Add(new KeyValuePair<string, string>(Keys.Space, "SPACE"));
            list.Add(new KeyValuePair<string, string>(Keys.Tab, "TAB"));
            list.Add(new KeyValuePair<string, string>("!", "1")); // Shift + 1 recognises the 1
            list.Add(new KeyValuePair<string, string>("$", "4")); // Shift + 4 recognises the 4

                // @wip - ALT + 3 comes up blank!
                //list.Add(new KeyValuePair<string, string>("#", ""));

            list.Add(new KeyValuePair<string, string>(Keys.Alt, "ALT"));
            list.Add(new KeyValuePair<string, string>("`", "BACK_QUOTE"));
            list.Add(new KeyValuePair<string, string>("'", "QUOTE"));
            list.Add(new KeyValuePair<string, string>(Keys.Subtract, "SUBTRACT"));

                // @wip - & _ also come up blank!
                //list.Add(new KeyValuePair<string, string>("_", ""));
                //list.Add(new KeyValuePair<string, string>("-", ""));

            list.Add(new KeyValuePair<string, string>(Keys.Pause, "PAUSE"));
            list.Add(new KeyValuePair<string, string>(Keys.ArrowLeft, "LEFT"));
            list.Add(new KeyValuePair<string, string>(Keys.Backspace, "BACK_SPACE"));
            list.Add(new KeyValuePair<string, string>(Keys.Delete, "DELETE"));
            list.Add(new KeyValuePair<string, string>(Keys.Escape, "ESCAPE"));

            list.Add(new KeyValuePair<string, string>(Keys.ArrowDown, "DOWN"));
            // @wip ENTER jumps off the screen very quickly where as the other descriptions do not.
            //list.Add(new KeyValuePair<string, string>(Keys.Enter, "ENTER"));

            // @wip What if I press down & enter to select my previous string "r"?

            foreach (KeyValuePair<string, string> pressedKey in list)
            {
                TestContext.WriteLine("Testing Key Stroke: " + pressedKey);
                page.FindElement(By.XPath(inputBoxXPath)).SendKeys(pressedKey.Key);
                page.FindElement(By.XPath(GetResult(pressedKey.Value)));
            }
        }

        [TearDown]
        public void CloseChrome()
        {
            page.Close();
        }
    }
}
