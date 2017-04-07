namespace ASPProjekt.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;

    [Authorize]
    public class BinsController : Controller
    {
        private readonly IRepository db;
        private IControllerIdentity identity;
        private bool reloadUserIdentity = false;

        public BinsController()
        {
            this.db = new Repository(new ApplicationDbContext());
            this.reloadUserIdentity = true;
        }

        public BinsController(IRepository repository, IControllerIdentity identity)
        {
            this.db = repository;
            this.identity = identity;
            this.reloadUserIdentity = false;
        }

        // GET: Bins/Create
        public ActionResult Create()
        {
            return this.View("Create");
        }

        // POST: Bins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Bin bin)
        {
            if (this.ModelState.IsValid)
            {
                if(this.reloadUserIdentity) this.identity = new ControllerIdentity(this.User.Identity);
                bin.ApplicationUserId = this.identity.GetUserId();
                this.db.Add(bin);
                return this.RedirectToAction("Details", "Account", new { userName = this.identity.GetUserName() });
            }

            return this.View("Create", bin);
        }

        // GET: Bins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bin = this.db.Bins.FirstOrDefault(x => x.Id == id.Value);
            if (bin == null)
            {
                throw new WebException();
            }

            return this.View("Delete", bin);
        }

        // POST: Bins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (this.reloadUserIdentity) this.identity = new ControllerIdentity(this.User.Identity);
            var bin = this.db.Bins.FirstOrDefault(x => x.Id == id);
            this.db.Remove(bin);
            return this.RedirectToAction("Details", "Account", new { userName = this.identity.GetUserName() });
        }

        // GET: Bins/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bin = this.db.Bins.FirstOrDefault(x => x.Id == id.Value);
            if (bin == null)
            {
                return this.HttpNotFound();
            }

            return this.View("Details", bin);
        }

        // GET: Bins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var bin = this.db.Bins.FirstOrDefault(x => x.Id == id.Value);
            if (bin == null)
            {
                return this.HttpNotFound();
            }

            return this.View("Edit", bin);
        }

        // POST: Bins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Bin bin)
        {
            if (this.ModelState.IsValid)
            {
                if (this.reloadUserIdentity) this.identity = new ControllerIdentity(this.User.Identity);
                bin.ApplicationUserId = this.identity.GetUserId();
                this.db.Edit(bin);
                return this.RedirectToAction("Details", "Account", new { userName = this.identity.GetUserName() });
            }

            return this.View("Edit", bin);
        }

        // GET: Bins
        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View("Index", this.db.Bins.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}