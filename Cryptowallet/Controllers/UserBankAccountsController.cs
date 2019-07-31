using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CryptowalletDb;
using CryptowalletDb.Domain;

namespace Cryptowallet.Controllers
{
    public class UserBankAccountsController : Controller
    {
        private CryptowalletDbContext db = new CryptowalletDbContext();

        // GET: UserBankAccounts
        public ActionResult Index()
        {
            var userBankAccounts = db.UserBankAccounts.Include(u => u.User);
            return View(userBankAccounts.ToList());
        }

        // GET: UserBankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBankAccount userBankAccount = db.UserBankAccounts.Find(id);
            if (userBankAccount == null)
            {
                return HttpNotFound();
            }
            return View(userBankAccount);
        }

        // GET: UserBankAccounts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name");
            return View();
        }

        // POST: UserBankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountId,UserId,Currency,Amount")] UserBankAccount userBankAccount)
        {
            if (ModelState.IsValid)
            {
                db.UserBankAccounts.Add(userBankAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", userBankAccount.UserId);
            return View(userBankAccount);
        }

        // GET: UserBankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBankAccount userBankAccount = db.UserBankAccounts.Find(id);
            if (userBankAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", userBankAccount.UserId);
            return View(userBankAccount);
        }

        // POST: UserBankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountId,UserId,Currency,Amount")] UserBankAccount userBankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userBankAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "Name", userBankAccount.UserId);
            return View(userBankAccount);
        }

        // GET: UserBankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserBankAccount userBankAccount = db.UserBankAccounts.Find(id);
            if (userBankAccount == null)
            {
                return HttpNotFound();
            }
            return View(userBankAccount);
        }

        // POST: UserBankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserBankAccount userBankAccount = db.UserBankAccounts.Find(id);
            db.UserBankAccounts.Remove(userBankAccount);
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
