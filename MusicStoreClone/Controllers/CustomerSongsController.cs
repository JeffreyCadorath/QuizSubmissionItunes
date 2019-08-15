using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStoreClone;

namespace MusicStoreClone.Controllers
{
    public class CustomerSongsController : Controller
    {
        private ITunesDBEntities db = new ITunesDBEntities();

        // GET: CustomerSongs
        public ActionResult Index()
        {
            var customerSongs = db.CustomerSongs.Include(c => c.Customer).Include(c => c.Song);
            return View(customerSongs.ToList());
        }

        // GET: CustomerSongs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSong customerSong = db.CustomerSongs.Find(id);
            if (customerSong == null)
            {
                return HttpNotFound();
            }
            return View(customerSong);
        }

        // GET: CustomerSongs/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName");
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Title");
            return View();
        }

        // POST: CustomerSongs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SongId,CustomerId")] CustomerSong customerSong)
        {
            if (ModelState.IsValid)
            {
                db.CustomerSongs.Add(customerSong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", customerSong.CustomerId);
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Title", customerSong.SongId);
            return View(customerSong);
        }

        // GET: CustomerSongs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSong customerSong = db.CustomerSongs.Find(id);
            if (customerSong == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", customerSong.CustomerId);
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Title", customerSong.SongId);
            return View(customerSong);
        }

        // POST: CustomerSongs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SongId,CustomerId")] CustomerSong customerSong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerSong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "UserName", customerSong.CustomerId);
            ViewBag.SongId = new SelectList(db.Songs, "Id", "Title", customerSong.SongId);
            return View(customerSong);
        }

        // GET: CustomerSongs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerSong customerSong = db.CustomerSongs.Find(id);
            if (customerSong == null)
            {
                return HttpNotFound();
            }
            return View(customerSong);
        }

        // POST: CustomerSongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerSong customerSong = db.CustomerSongs.Find(id);
            db.CustomerSongs.Remove(customerSong);
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
