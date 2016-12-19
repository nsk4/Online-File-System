using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineFileSystem.Models;

namespace OnlineFileSystem.Controllers
{
    public class FileContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FileContents
        public ActionResult Index()
        {
            return View(db.FileContents.ToList());
        }

        // GET: FileContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileContent fileContent = db.FileContents.Find(id);
            if (fileContent == null)
            {
                return HttpNotFound();
            }
            return View(fileContent);
        }

        // GET: FileContents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileContentId,Data")] FileContent fileContent)
        {
            if (ModelState.IsValid)
            {
                db.FileContents.Add(fileContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fileContent);
        }

        // GET: FileContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileContent fileContent = db.FileContents.Find(id);
            if (fileContent == null)
            {
                return HttpNotFound();
            }
            return View(fileContent);
        }

        // POST: FileContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileContentId,Data")] FileContent fileContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fileContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fileContent);
        }

        // GET: FileContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileContent fileContent = db.FileContents.Find(id);
            if (fileContent == null)
            {
                return HttpNotFound();
            }
            return View(fileContent);
        }

        // POST: FileContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileContent fileContent = db.FileContents.Find(id);
            db.FileContents.Remove(fileContent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
