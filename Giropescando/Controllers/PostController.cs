using Giropescando.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giropescando.Controllers
{
    public class PostController : Controller
    {
        private readonly ModelDbContext _context;

        public PostController()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ModelDBcontext"].ConnectionString;
            _context = new ModelDbContext(connectionString);
        }

        public PostController(ModelDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            ViewBag.HideCarousel = true;
            var posts = _context.Posts.Include("User").ToList();

            // Calcola il conteggio dei "Mi piace" per ogni post e memorizzalo in un Dizionario
            var miPiaceCounts = new Dictionary<int, int>();
            foreach (var post in posts)
            {
                miPiaceCounts[post.IdPost] = _context.MiPiace.Count(m => m.IdPost == post.IdPost);
            }

            // Passa il Dizionario alla vista utilizzando ViewBag
            ViewBag.MiPiaceCounts = miPiaceCounts;

            // Imposta il modello di login
            ViewBag.LoginModel = new LoginViewModel();

            return View(posts);
        }





        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize] // Assicurati che l'utente sia loggato
        public ActionResult Create(Post post, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Ottieni l'utente attualmente loggato
                var user = User.Identity.Name;

                // Aggiungi il nome utente al post
                post.User = _context.USER.FirstOrDefault(u => u.Nome == user);

                // Imposta la data di pubblicazione sulla data e ora correnti
                post.DataPubblicazione = DateTime.Now;

                // Gestisci il caricamento del file
                if (file != null && file.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        post.Immagine = reader.ReadBytes(file.ContentLength);
                    }
                }

                // Salva il post nel database
                _context.Posts.Add(post);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(post);
        }


        [HttpPost]
        [Authorize] // Assicurati che l'utente sia loggato
        public ActionResult CreaCommento(int idPost, string contenuto)
        {
            var commento = new Commento
            {
                IdPost = idPost,
                Contenuto = contenuto,
                DataPubblicazione = DateTime.Now,
                IdUser = _context.USER.FirstOrDefault(u => u.Nome == User.Identity.Name).IdUser
            };

            _context.Commenti.Add(commento);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize] // Assicurati che l'utente sia loggato
        public ActionResult AggiungiMiPiace(int idPost)
        {
            // Ottieni l'utente attualmente loggato
            var user = _context.USER.FirstOrDefault(u => u.Nome == User.Identity.Name);

            if (user == null)
            {
                // Gestisci il caso in cui l'utente non è loggato
                return Json(new { success = false, message = "Devi essere loggato per mettere 'Mi piace' a un post." });
            }

            // Cerca un "Mi piace" esistente
            var miPiaceEsistente = _context.MiPiace.FirstOrDefault(m => m.IdPost == idPost && m.IdUser == user.IdUser);

            if (miPiaceEsistente != null)
            {
                // Se l'utente ha già messo "Mi piace" a questo post, rimuovi il "Mi piace"
                _context.MiPiace.Remove(miPiaceEsistente);
            }
            else
            {
                // Altrimenti, aggiungi un nuovo "Mi piace"

                // Verifica se il post esiste
                var post = _context.Posts.Find(idPost);
                if (post == null)
                {
                    // Gestisci il caso in cui il post non esiste
                    return HttpNotFound();
                }

                // Crea un nuovo oggetto MiPiace
                var miPiace = new MiPiace
                {
                    IdUser = user.IdUser,
                    IdPost = idPost
                };

                // Aggiungi l'oggetto MiPiace al database
                _context.MiPiace.Add(miPiace);
            }

            _context.SaveChanges();

            // Calcola il nuovo conteggio dei "Mi piace" per il post
            var miPiaceCount = _context.MiPiace.Count(m => m.IdPost == idPost);

            // Restituisci i dati come risposta JSON
            return Json(new { success = true, idPost = idPost, miPiaceCount = miPiaceCount });
        }

    }
}


