using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CryptowalletProject.Models;

namespace CryptowalletProject.Controllers
{
    public class UsersBankAccountsController : Controller
    {
        private CryptoWalletDbTestEntities db = new CryptoWalletDbTestEntities();

        // GET: UsersBankAccounts
        public ActionResult Index()
        {
            var usersBankAccounts = db.UsersBankAccounts.Include(u => u.User);
            return View(usersBankAccounts.ToList());
        }

        // GET: UsersBankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersBankAccount usersBankAccount = db.UsersBankAccounts.Find(id);
            if (usersBankAccount == null)
            {
                return HttpNotFound();
            }
            return View(usersBankAccount);
        }

        // GET: UsersBankAccounts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name");
            return View();
        }

        // POST: UsersBankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,UserId,Currency,Amount")] UsersBankAccount usersBankAccount)
        {
            if (ModelState.IsValid)
            {
                db.UsersBankAccounts.Add(usersBankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", usersBankAccount.UserId);
            return View(usersBankAccount);
        }

        // GET: UsersBankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersBankAccount usersBankAccount = db.UsersBankAccounts.Find(id);
            if (usersBankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", usersBankAccount.UserId);
            return View(usersBankAccount);
        }

        // POST: UsersBankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,UserId,Currency,Amount")] UsersBankAccount usersBankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usersBankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", usersBankAccount.UserId);
            return View(usersBankAccount);
        }

        // GET: UsersBankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersBankAccount usersBankAccount = db.UsersBankAccounts.Find(id);
            if (usersBankAccount == null)
            {
                return HttpNotFound();
            }
            return View(usersBankAccount);
        }

        // POST: UsersBankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsersBankAccount usersBankAccount = db.UsersBankAccounts.Find(id);
            db.UsersBankAccounts.Remove(usersBankAccount);
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
