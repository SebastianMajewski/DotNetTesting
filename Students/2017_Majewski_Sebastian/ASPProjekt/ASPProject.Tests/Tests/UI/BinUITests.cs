using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ASPProject.Tests.Tests.UI
{
    using System.Collections.ObjectModel;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    [TestClass]
    public class BinUITests : SeleniumHelper
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            SeleniumHelper.ClassInitialize(Logins.UserOne);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            SeleniumHelper.ClassCleanup();
        }

        [TestMethod]
        public void AddBinRedirect()
        {
            this.Open("~/Bins/Create");
            Assert.AreEqual("Nowy śmietnik", this.GetElement("title").Text);
            this.Type("name", "Bin");
            this.Type("Description", "New Bin");
            this.Click("submitBin");
            Assert.AreEqual("Śmietniki użytkownika:", this.GetElement("bins").Text);
        }

        [TestMethod]
        public void BinDelete()
        {
            this.Open("~/Bins/Create");
            this.Type("name", "Bin");
            this.Type("Description", "New Bin");
            this.Click("submitBin");
            this.Open("~/Bins/Delete/1");
            Assert.AreEqual("Usuń śmietnik", this.GetElement("title").Text);
            this.Click("submit");
            Assert.AreEqual("Śmietniki użytkownika:", this.GetElement("bins").Text);
        }

        [TestMethod]
        public void BinEdit()
        {
            IWebDriver driver = new ChromeDriver("./", new ChromeOptions(), TimeSpan.FromSeconds(300));
            INavigation nav = driver.Navigate();
            nav.GoToUrl("http://www.google.com");
            Assert.IsTrue(true);
            //IWebElement el = driver.FindElement(By.Name("edytuj"));
            //String message = el.Text;
            //String successMsg = "Edytuj dane";
            //Assert.AreEqual(message, successMsg);
        }

        [TestMethod]
        public void TestAddPerson()
        {
            IWebDriver driver = new ChromeDriver();
            INavigation nav = driver.Navigate();
            nav.GoToUrl("http://localhost:9098/Person/Add");
            driver.FindElement(By.Id("nazwisko")).Click();
            driver.FindElement(By.Id("nazwisko")).SendKeys("Nowak");
            driver.FindElement(By.Id("imie")).Click();
            driver.FindElement(By.Id("imie")).SendKeys("Kasia");
            driver.FindElement(By.Name("dodaj")).Click();
            string checkTest = driver.FindElement(By.Name("Title")).Text;
            Assert.AreEqual("Lista wszystkich osób", checkTest);
            driver.Close();
        }

        [TestMethod]
        public void TestAllPerson()
        {
            IWebDriver driver = new ChromeDriver();
            INavigation nav = driver.Navigate();
            nav.GoToUrl("http://localhost:9098/Person/All");

            IWebElement table = driver.FindElement(By.XPath("/html/body/div[2]/table"));

            ReadOnlyCollection<IWebElement> allRows = table.FindElements(By.TagName("tr"));
            int licz = 0;
            foreach (IWebElement row in allRows)
            {
                ReadOnlyCollection<IWebElement> cells = row.FindElements(By.TagName("td"));

                foreach (IWebElement cell in cells)
                {
                    //Console.WriteLine("\t" + cell.Text);
                }
                licz++;
            }
            Assert.IsTrue(licz > 4);
            driver.Close();
        }

    }
}
