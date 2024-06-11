using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Giropescando.Models; // Assicurati che questo namespace contenga la tua classe Cards

namespace Giropescando.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext _context;

        public HomeController()
        {
            _context = new ModelDbContext();
        }

        public ActionResult Index()
        {
            var loginModel = new LoginViewModel
            {
                Username = "", // Imposta il valore iniziale per Username
                Pass = ""  // Imposta il valore iniziale per Password
            };
            ViewBag.LoginModel = loginModel;
            return View();
        }


        public ActionResult Racconti_di_pesca()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ToastType"] = "warning";
                TempData["ToastMessage"] = "Devi effettuare il login per accedere a questa sezione.";
                return RedirectToAction("Index", "Home");
            }

            MyViewModel model = new MyViewModel();
            model.Login = new LoginViewModel(); // popola il LoginViewModel con i dati necessari
            model.Cards = _context.Cards.ToList();

            return View(model);
        }



        public ActionResult Corsi()
        {
            return View();
        }

        public ActionResult Dettagli(int id)
        {
            var card = _context.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cards card, string mapUrl)
        {
            if (ModelState.IsValid)
            {
                // Codifica l'URL della mappa prima di salvarlo nel modello
                string encodedMapUrl = HttpUtility.UrlEncode(mapUrl);
                card.MapUrl = encodedMapUrl;

                _context.Cards.Add(card);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(card);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cards card = _context.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }

            return View(card);
        }

        public ActionResult LoginPartial()
        {
            LoginViewModel model = new LoginViewModel();
            // popola il modello con i dati necessari
            return PartialView("~/Views/Shared/_LoginPartial.cshtml", model);
        }

    }

}

