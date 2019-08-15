using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStoreClone;

namespace MusicStoreClone.Controllers
{
    public class CustomersController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private ITunesDBEntities db = new ITunesDBEntities();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Birthday")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Birthday")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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

        public ActionResult SeeCustomerSongs()
        {
            var customers = db.Customers.ToList();
            Logger.Info("Customer Searched For a specified Customer");
            ViewBag.CustomerId = new SelectList(customers, "Id", "Id");
            return View();
        }

        [HttpPost]
        public ActionResult SeeCustomerSongs(int? CustomerId)
        {
            try
            {
                var result = db.Database.SqlQuery<SeeCustomersSongs>("EXEC SeeCustomerSong @CustomerId").ToList();
                new SqlParameter("CustomerId", CustomerId);
                Logger.Info("Customer Searched For their song list");
                return View(result);
            }
            catch
            {
                Logger.Error("The CustomerID you looked for was not valid");
                throw new Exception();
            }
        }

        public ActionResult SeeSongsCustomers(int? songId = 2)
        {
            var result = db.Database.SqlQuery<SeeSongs>("EXEC SongsBoughtByCustomer @SongId").ToList();
            new SqlParameter("SongId", songId);
            Logger.Info("Customer Searched to see what songs");
            return View(result);
        }


        public class SeeSongs
        {
            public string Song { get; set; }
            public string Customers { get; set; }
        }
        public class SeeCustomersSongs
        {
            public string Song { get; set; }
            public string Username { get; set; }
        }
    }
}
