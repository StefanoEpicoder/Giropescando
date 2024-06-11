using Giropescando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Giropescando.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            // Memorizza l'URL di ritorno come un parametro del metodo
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

            if (USER.Autenticato(u.Username, u.Pass))
            {
                var userData = u.IdUser.ToString();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    u.Username,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userData,
                    FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Benvenuto, " + u.Username + "!";

                // Reindirizza l'utente alla pagina "Articolo"
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Username o password non corretti";
                return View();
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


