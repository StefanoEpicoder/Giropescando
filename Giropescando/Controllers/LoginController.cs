using Giropescando.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace Giropescando.Controllers
{
    public class LoginController : Controller
    {
        private ModelDbContext _context;

        public LoginController()
        {
            _context = new ModelDbContext();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(USER u)
        {
            if (u == null || string.IsNullOrEmpty(u.Username) || string.IsNullOrEmpty(u.Pass))
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Username o password non forniti";
                return View();
            }

            // Rimuovi l'hashing della password e confronta direttamente la password in chiaro
            var authenticatedUser = _context.USER.FirstOrDefault(dbUser => dbUser.Username == u.Username && dbUser.Pass == u.Pass);

            if (authenticatedUser != null)
            {
                var userData = authenticatedUser.IdUser.ToString();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    authenticatedUser.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userData,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Benvenuto, " + authenticatedUser.Username + "!";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Username o password non corretti";
                return RedirectToAction("Index", "Home");
            }
        }



        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Logout effettuato con successo!";
            return Redirect(FormsAuthentication.DefaultUrl);
        }

        public ActionResult LoginPartial()
        {
            return PartialView("~/Views/Shared/LoginPartial.cshtml");
        }
    }
}



