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

            ReadOnlyCollection<IWebElement> cells = Driver.FindElements(By.ClassName("link"));
            this.Open("~/Bins/Delete/" + cells[0].GetAttribute("id"));
            Assert.AreEqual("Usuń śmietnik", this.GetElement("title").Text);
            this.Click("submit");
            Assert.AreEqual("Śmietniki użytkownika:", this.GetElement("bins").Text);
        }

        [TestMethod]
        public void BinEdit()
        {
            this.Open("~/Bins/Create");
            this.Type("name", "Bin");
            this.Type("Description", "New Bin");
            this.Click("submitBin");

            ReadOnlyCollection<IWebElement> cells = Driver.FindElements(By.ClassName("link"));
            this.Open("~/Bins/Edit/" + cells[0].GetAttribute("id"));
            this.Type("name", "Bin2");
            this.Type("description", "New Bin2");
            this.Click("submit");
            Assert.AreEqual("Śmietniki użytkownika:", this.GetElement("bins").Text);
        }

        [TestMethod]
        public void TestAllBins()
        {
            this.Open("~/User/Admin");
            ReadOnlyCollection<IWebElement> cells = Driver.FindElements(By.ClassName("link"));
            for (var i = 0; i < 4; i++)
            {
                this.Open("~/Bins/Create");
                this.Type("name", "Bin");
                this.Type("Description", "New Bin");
                this.Click("submitBin");
            }
            this.Open("~/User/Admin");
            ReadOnlyCollection<IWebElement> cells2 = Driver.FindElements(By.ClassName("link"));          
            Assert.IsTrue(cells.Count + 4 == cells2.Count);
        }

    }
}
