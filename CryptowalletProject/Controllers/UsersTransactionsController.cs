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
    public class UsersTransactionsController : Controller
    {
        private CryptoWalletDbTestEntities db = new CryptoWalletDbTestEntities();

        // GET: UsersTransactions
        public ActionResult Index()
        {
            var usersTransactions = db.UsersTransactions.Include(u => u.UsersBankAccount).Include(u => u.UsersBankAccount1);
            return View(usersTransactions.ToList());
        }

        // GET: UsersTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersTransaction usersTransaction = db.UsersTransactions.Find(id);
            if (usersTransaction == null)
            {
                return HttpNotFound();
            }
            return View(usersTransaction);
        }

        // GET: UsersTransactions/Create
        public ActionResult Create()
        {
            ViewBag.FromAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId");
            ViewBag.ToAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId");
            return View();
        }

        // POST: UsersTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionsId,FromAccount,ToAccount,Amount,CurrencyRate,TransactionData")] UsersTransaction usersTransaction)
        {
            if (ModelState.IsValid)
            {
                db.UsersTransactions.Add(usersTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.FromAccount);
            ViewBag.ToAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.ToAccount);
            return View(usersTransaction);
        }

        // GET: UsersTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersTransaction usersTransaction = db.UsersTransactions.Find(id);
            if (usersTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.FromAccount);
            ViewBag.ToAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.ToAccount);
            return View(usersTransaction);
        }

        // POST: UsersTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionsId,FromAccount,ToAccount,Amount,CurrencyRate,TransactionData")] UsersTransaction usersTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usersTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.FromAccount);
            ViewBag.ToAccount = new SelectList(db.UsersBankAccounts, "AccountId", "AccountId", usersTransaction.ToAccount);
            return View(usersTransaction);
        }

        // GET: UsersTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersTransaction usersTransaction = db.UsersTransactions.Find(id);
            if (usersTransaction == null)
            {
                return HttpNotFound();
            }
            return View(usersTransaction);
        }

        // POST: UsersTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsersTransaction usersTransaction = db.UsersTransactions.Find(id);
            db.UsersTransactions.Remove(usersTransaction);
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
