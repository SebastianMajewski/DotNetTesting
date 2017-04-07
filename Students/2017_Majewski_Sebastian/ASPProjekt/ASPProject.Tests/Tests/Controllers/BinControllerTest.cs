using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ASPProject.Tests.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using ASPProject.Tests.Fakes;

    using ASPProjekt.Controllers;
    using ASPProjekt.Models;

    [TestClass]
    public class BinControllerTest
    {
        private BinsController controller = null;
        private IRepository repository = null;
        private IControllerIdentity identity = null;

        [TestInitialize]
        public void SetUp()
        {
            this.repository = new FakeRepository();
            this.identity = new FakeIdentity();
            var bin = new Bin { Trash = new List<Trash>(), Id = 1, Description = "Description", Name = "Bin1", ApplicationUserId = "1" };
            var bin2 = new Bin { Trash = new List<Trash>(), Id = 2, Description = "Description", Name = "Bin2", ApplicationUserId = "1" };
            var bin3 = new Bin { Trash = new List<Trash>(), Id = 3, Description = "Description", Name = "Bin3", ApplicationUserId = "1" };
            this.repository.Add(bin);
            this.repository.Add(bin2);
            this.repository.Add(bin3);
            this.controller = new BinsController(this.repository, this.identity);
        }

        [TestMethod]
        public void CreateView()
        {
            var ar = this.controller.Create() as ViewResult;
            Assert.AreEqual("Create", ar.ViewName);
        }

        [TestMethod]
        public void CreateAdd()
        {
            var bin4 = new Bin { Trash = new List<Trash>(), Id = 4, Description = "Description", Name = "Bin4", ApplicationUserId = "1" };
            this.controller.Create(bin4);
            Assert.AreEqual(4, this.repository.Bins.Count());
        }

        [TestMethod]
        public void CreateAddInvalid()
        {
            var bin4 = new Bin { Trash = new List<Trash>(), Id = 4, Description = "Description", Name = "B", ApplicationUserId = "1" };
            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => bin4, bin4.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
            };
            new DefaultModelBinder().BindModel(new ControllerContext(), modelBinder);
            this.controller.ModelState.Clear();
            this.controller.ModelState.Merge(modelBinder.ModelState);
            var ar = this.controller.Create(bin4) as ViewResult;
            Assert.AreEqual("Create", ar.ViewName);
            Assert.IsFalse(ar.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void DeleteBadRequest()
        {
            var ar = this.controller.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ar.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void DeleteBadId()
        {
            var ar = this.controller.Delete(10);
            Assert.AreEqual(typeof(HttpNotFoundResult), ar.GetType());
        }

        [TestMethod]
        public void DeleteIdOk()
        {
            var ar = this.controller.Delete(1) as ViewResult;
            var bin = this.repository.Bins.FirstOrDefault(x => x.Id == 1);
            Assert.AreEqual("Delete", ar.ViewName);
            Assert.AreEqual(bin, ar.Model);
        }

        [TestMethod]
        public void DeleteConfirmed()
        {
            this.controller.DeleteConfirmed(1);
            Assert.AreEqual(2, this.repository.Bins.Count());
        }

        [TestMethod]
        public void DeleteConfirmedRedirect()
        {
            var ar = this.controller.DeleteConfirmed(1) as RedirectToRouteResult;
            Assert.AreEqual("Details", ar.RouteValues["action"]);
            Assert.AreEqual("Account", ar.RouteValues["controller"]);
        }

        [TestMethod]
        public void DetailsBadRequest()
        {
            var ar = this.controller.Details(null) as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ar.StatusCode);
        }

        [TestMethod]
        public void DetailsBadId()
        {
            var ar = this.controller.Details(10);
            Assert.AreEqual(typeof(HttpNotFoundResult), ar.GetType());
        }

        [TestMethod]
        public void DetailsIdOk()
        {
            var ar = this.controller.Details(1) as ViewResult;
            var bin = this.repository.Bins.FirstOrDefault(x => x.Id == 1);
            Assert.AreEqual("Details", ar.ViewName);
            Assert.AreEqual(bin, ar.Model);
        }

        [TestMethod]
        public void EditBadRequest()
        {
            var ar = this.controller.Edit(null as int?) as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, ar.StatusCode);
        }

        [TestMethod]
        public void EditBadId()
        {
            var ar = this.controller.Edit(10);
            Assert.AreEqual(typeof(HttpNotFoundResult), ar.GetType());
        }

        [TestMethod]
        public void EditIdOk()
        {
            var ar = this.controller.Edit(1) as ViewResult;
            var bin = this.repository.Bins.FirstOrDefault(x => x.Id == 1);
            Assert.AreEqual("Edit", ar.ViewName);
            Assert.AreEqual(bin, ar.Model);
        }

        [TestMethod]
        public void EditConfirmed()
        {
            var bin = this.repository.Bins.First();
            bin.Name = "BinName";
            this.controller.Edit(bin);
            Assert.AreEqual("BinName", this.repository.Bins.First(x => x.Id == bin.Id).Name);
        }

        [TestMethod]
        public void EditConfirmedRedirect()
        {
            var bin = this.repository.Bins.First();
            bin.Name = "BinName";
            var ar = this.controller.Edit(bin) as RedirectToRouteResult;
            Assert.AreEqual("Details", ar.RouteValues["action"]);
            Assert.AreEqual("Account", ar.RouteValues["controller"]);
        }

        [TestMethod]
        public void EditInvalid()
        {
            var bin4 = new Bin { Trash = new List<Trash>(), Id = 1, Description = "Description", Name = "B", ApplicationUserId = "1" };
            var modelBinder = new ModelBindingContext()
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => bin4, bin4.GetType()),
                ValueProvider = new NameValueCollectionValueProvider(new NameValueCollection(), CultureInfo.InvariantCulture)
            };
            new DefaultModelBinder().BindModel(new ControllerContext(), modelBinder);
            this.controller.ModelState.Clear();
            this.controller.ModelState.Merge(modelBinder.ModelState);
            var ar = this.controller.Edit(bin4) as ViewResult;
            Assert.AreEqual("Edit", ar.ViewName);
            Assert.IsFalse(ar.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void IndexReturnView()
        {
            var ar = this.controller.Index() as ViewResult;
            Assert.AreEqual("Index", ar.ViewName);
            CollectionAssert.AreEquivalent(this.repository.Bins.ToList(), (List<Bin>)ar.Model);
        }
    }
}
