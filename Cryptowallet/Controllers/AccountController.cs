using Cryptowallet.Models;
using CryptowalletDb;
using CryptowalletDb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Cryptowallet.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                using (CryptowalletDbContext ctx = new CryptowalletDbContext())
                {
                   
                    CryptowalletDb.Domain.User user = ctx.Users.FirstOrDefault(u => u.Email == viewModel.Username && u.Password == viewModel.Password);
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(viewModel.Username, true);
                        return RedirectToAction("Profil", "BankAccounts");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect user or password");
                        return View(viewModel);
                    }

                }
            }
            else
            {
                return View(viewModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult RegisterUser()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(RegisterViewModel viewModel)
        {
            using (CryptowalletDbContext ctx = new CryptowalletDbContext())
            {
                ///de facut LOG IN
                User newUser = ctx.Users.FirstOrDefault(a=>a.Email == viewModel.Email);

                if (ModelState.IsValid)
                {
                    if (newUser != null)
                    {
                        ModelState.AddModelError("", "Already exists an user with this email");
                        return View(viewModel);

                    }

                    if (viewModel.Password != viewModel.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Your password and confirm password doesn`t match");
                        return View(viewModel);
                    }
                

                    newUser = new User
                    {
                        Email=viewModel.Email,
                        Name=viewModel.Name,
                        Password=viewModel.Password
                    };
                    ctx.Users.Add(newUser);
                    ctx.SaveChanges();

                    LoginViewModel login = new LoginViewModel
                    {
                        Username = newUser.Email,
                        Password = newUser.Password
                    }; 
                    return RedirectToAction("Index", "Home");
                }

                    return View(newUser);

                }
            

        }
    }
}