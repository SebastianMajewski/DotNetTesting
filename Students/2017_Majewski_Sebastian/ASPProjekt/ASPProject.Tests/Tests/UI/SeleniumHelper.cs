
namespace ASPProject.Tests.Tests.UI
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;

    public class SeleniumHelper
    {
        public const string BaseUrl = "http://aspprojekt.azurewebsites.net";
        private const string VirtualPath = "";
        private const int TimeOut = 1000;
        private static readonly IWebDriver StaticDriver = CreateDriverInstance();
        private static Login currentlyLoggedInAs;
        private static int testClassesRunning;

        static SeleniumHelper()
        {
            StaticDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TimeOut);
            StaticDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(TimeOut);
            StaticDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(TimeOut);
        }

        public IWebDriver Driver => StaticDriver;

        // Pass in null if want to run your test-case without logging in.
        public static void ClassInitialize(Login login = null)
        {
            testClassesRunning++;
            if (login == null)
            {
                Logoff();
            }
            else if (!IsCurrentlyLoggedInAs(login))
            {
                Logon(login);
            }
        }

        public static void ClassCleanup()
        {
            try
            {
                testClassesRunning--;
                if (testClassesRunning == 0)
                {
                    StaticDriver.Close();
                }
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public void Open(string url)
        {
            this.Driver.Navigate().GoToUrl(BaseUrl + VirtualPath + url.Trim('~'));
        }

        public void Click(string id)
        {
            this.Click(By.Id(id));
        }

        public void Click(By locator)
        {
            this.Driver.FindElement(locator).Click();
        }

        // public void ClickAndWait(string id, string newUrl)
        // {
        //   ClickAndWait(By.Id(id), newUrl);
        // }

        // <summary>
        // Use when you are navigating via a hyper-link and need for the page to fully load before 
        // moving further.  
        // </summary>
        // public void ClickAndWait(By locator, string newUrl)
        // {
        // Driver.FindElement(locator).Click();
        // WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeOut));
        // wait.Until(d => d.Url.Contains(newUrl.Trim('~')));
        // }

        public void AssertCurrentPage(string pageUrl)
        {
            var absoluteUrl = new Uri(new Uri(BaseUrl), VirtualPath + pageUrl.Trim('~')).ToString();
            Assert.AreEqual(absoluteUrl, Driver.Url);
        }

        public void AssertTextContains(string id, string text)
        {
            this.AssertTextContains(By.Id(id), text);
        }

        public void AssertTextContains(By locator, string text)
        {
            Assert.IsTrue(this.Driver.FindElement(locator).Text.Contains(text));
        }

        public void AssertTextEquals(string id, string text)
        {
            this.AssertTextEquals(By.Id(id), text);
        }

        public void AssertTextEquals(By locator, string text)
        {
            Assert.AreEqual(text, this.Driver.FindElement(locator).Text);
        }

        public void AssertValueContains(string id, string text)
        {
            this.AssertValueContains(By.Id(id), text);
        }

        public void AssertValueContains(By locator, string text)
        {
            Assert.IsTrue(this.GetValue(locator).Contains(text));
        }

        public void AssertValueEquals(string id, string text)
        {
            this.AssertValueEquals(By.Id(id), text);
        }

        public void AssertValueEquals(By locator, string text)
        {
            Assert.AreEqual(text, GetValue(locator));
        }

        public IWebElement GetElement(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        public string GetValue(By locator)
        {
            return Driver.FindElement(locator).GetAttribute("value");
        }

        public string GetText(By locator)
        {
            return Driver.FindElement(locator).Text;
        }

        public void Type(string id, string text)
        {
            var element = GetElement(id);
            element.Clear();
            element.SendKeys(text);
        }

        public void Uncheck(string id)
        {
            Uncheck(By.Id(id));
        }

        public void Uncheck(By locator)
        {
            var element = Driver.FindElement(locator);
            if (element.Selected) element.Click();
        }

        // Selects an element from a drop-down list.
        public void Select(string id, string valueToBeSelected)
        {
            var options = GetElement(id).FindElements(By.TagName("option"));
            foreach (var option in options)
            {
                if (valueToBeSelected == option.Text) option.Click();
            }
        }

        private static IWebDriver CreateDriverInstance(string baseUrl = BaseUrl)
        {
            return new ChromeDriver("./", new ChromeOptions(), TimeSpan.FromSeconds(300));
        }

        private static bool IsCurrentlyLoggedInAs(Login login)
        {
            return currentlyLoggedInAs != null && currentlyLoggedInAs.Equals(login);
        }

        private static void Logon(Login login)
        {
            StaticDriver.Navigate().GoToUrl(BaseUrl + "/Account/Login");

            StaticDriver.FindElement(By.Id("email")).SendKeys(login.Username);
            StaticDriver.FindElement(By.Id("password")).SendKeys(login.Password);
            StaticDriver.FindElement(By.Id("loginbutton")).Click();
            currentlyLoggedInAs = login;
        }

        private static void Logoff()
        {
            StaticDriver.FindElement(By.Id("logout")).Click();
            currentlyLoggedInAs = null;
        }
    }
}

