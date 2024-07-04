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
            // Ordina i post per DataPubblicazione in ordine decrescente
            var posts = _context.Posts
                .Include("User")
                .Include("Commenti") // Assicurati di includere i commenti
                .OrderByDescending(p => p.DataPubblicazione)
                .ToList();

            foreach (var post in posts)
            {
                // Assicurati che i commenti siano ordinati dal più recente al più vecchio
                post.Commenti = post.Commenti.OrderByDescending(c => c.DataPubblicazione).ToList();
            }

            var miPiaceCounts = new Dictionary<int, int>();
            foreach (var post in posts)
            {
                miPiaceCounts[post.IdPost] = _context.MiPiace.Count(m => m.IdPost == post.IdPost);
            }

            ViewBag.MiPiaceCounts = miPiaceCounts;
            ViewBag.LoginModel = new LoginViewModel();

            return View(posts);
        }



        public ActionResult Create()
            {
                return View();
            }

        [HttpPost]
        [Authorize(Roles = "Utente,Amministratore")]

        public ActionResult Create(Post post, HttpPostedFileBase file)
            {
                if (ModelState.IsValid)
                {
                    var user = User.Identity.Name;
                    post.User = _context.USER.FirstOrDefault(u => u.Nome == user);
                    post.DataPubblicazione = DateTime.Now;

                    if (file != null && file.ContentLength > 0)
                    {
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            post.Immagine = reader.ReadBytes(file.ContentLength);
                        }
                    }

                    _context.Posts.Add(post);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(post);
            }

        [HttpPost]
        [Authorize(Roles = "Utente,Amministratore")]
        public ActionResult CreaCommento(int idPost, string contenuto)
        {
            // Verifica che il contenuto del commento non sia vuoto
            if (string.IsNullOrWhiteSpace(contenuto))
            {
                return Json(new { success = false, message = "Il contenuto del commento non può essere vuoto." });
            }

            // Cerca l'utente basandosi sul nome utente corrente
            var user = _context.USER.FirstOrDefault(u => u.Nome == User.Identity.Name);
            // Verifica che l'utente esista e abbia il ruolo corretto
            if (user == null || (user.Ruolo != "Utente" && user.Ruolo != "Amministratore"))
            {
                return Json(new { success = false, message = "Utente non autorizzato o non trovato." });
            }

            // Crea il nuovo commento con i dettagli forniti
            var commento = new Commento
            {
                IdPost = idPost,
                Contenuto = contenuto,
                DataPubblicazione = DateTime.Now,
                IdUser = user.IdUser // Usa l'IdUser dell'utente trovato
            };

            // Aggiunge il commento al contesto e salva le modifiche
            _context.Commenti.Add(commento);
            _context.SaveChanges();

            // Restituisce i dettagli del commento appena creato
            return Json(new { success = true, idCommento = commento.IdCommento, contenuto = commento.Contenuto, dataPubblicazione = commento.DataPubblicazione.ToShortDateString() });
        }




        [HttpPost]
        [Authorize(Roles = "Utente,Amministratore")]

        public ActionResult AggiungiMiPiace(int idPost)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Restituisce uno stato HTTP 401 Unauthorized se l'utente non è loggato
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized, "Devi essere loggato per mettere 'Mi piace' a un post.");
            }

            var user = _context.USER.FirstOrDefault(u => u.Nome == User.Identity.Name);
            // Non è più necessario controllare se user è null qui, poiché abbiamo già verificato l'autenticazione

            var miPiaceEsistente = _context.MiPiace.FirstOrDefault(m => m.IdPost == idPost && m.IdUser == user.IdUser);

            if (miPiaceEsistente != null)
            {
                _context.MiPiace.Remove(miPiaceEsistente);
            }
            else
            {
                var post = _context.Posts.Find(idPost);
                if (post == null)
                {
                    return HttpNotFound();
                }

                var miPiace = new MiPiace
                {
                    IdUser = user.IdUser,
                    IdPost = idPost
                };

                _context.MiPiace.Add(miPiace);
            }

            _context.SaveChanges();

            var miPiaceCount = _context.MiPiace.Count(m => m.IdPost == idPost);

            return Json(new { success = true, idPost = idPost, miPiaceCount = miPiaceCount });
        }


        [HttpPost]
        [Authorize(Roles = "Utente,Amministratore")]

        public ActionResult EliminaPost(int idPost)
        {
            var post = _context.Posts.FirstOrDefault(p => p.IdPost == idPost);

            if (post != null && post.User.Nome == User.Identity.Name)
            {
                // Prima elimina tutti i "Mi piace" e i commenti associati al post
                var miPiaceAssociati = _context.MiPiace.Where(m => m.IdPost == idPost);
                _context.MiPiace.RemoveRange(miPiaceAssociati);

                var commentiAssociati = _context.Commenti.Where(c => c.IdPost == idPost);
                _context.Commenti.RemoveRange(commentiAssociati);

                // Poi elimina il post
                _context.Posts.Remove(post);
                _context.SaveChanges();

                return Json(new { success = true, message = "Il post è stato eliminato con successo." });
            }
            else
            {
                return Json(new { success = false, message = "Non è stato possibile eliminare il post." });
            }
        }


        [HttpPost]
        [Authorize]
        public ActionResult EliminaCommento(int idCommento)
        {
            var commento = _context.Commenti.FirstOrDefault(c => c.IdCommento == idCommento);

            if (commento != null && commento.User.Nome == User.Identity.Name)
            {
                _context.Commenti.Remove(commento);
                _context.SaveChanges();

                return Json(new { success = true, idCommento = idCommento, message = "Il commento è stato eliminato con successo." });
            }
            else
            {
                return Json(new { success = false, message = "Non è stato possibile eliminare il commento." });
            }
        }

    }
}





