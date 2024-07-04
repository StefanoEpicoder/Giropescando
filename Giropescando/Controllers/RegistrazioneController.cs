using Giropescando.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics; // Aggiungi questo per il logging

namespace Giropescando.Controllers
{
    public class RegistrazioneController : Controller
    {
        private ModelDbContext _context;

        public RegistrazioneController()
        {
            _context = new ModelDbContext();
        }

        public RegistrazioneController(ModelDbContext context)
        {
            _context = context;
        }

        public ActionResult RegistraUtente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistraUtente(REGISTRAZIONE model)
        {
            if (ModelState.IsValid)
            {
                if (_context.USER.Any(u => u.Username == model.Username))
                {
                    TempData["ErrorMessage"] = "Username esistente.";
                    return RedirectToAction("Index", "Home");
                }

                // Rimuovi l'hashing e usa la password in chiaro
                var passwordInChiaro = model.Password;

                var user = new USER
                {
                    Username = model.Username,
                    Pass = passwordInChiaro, // Usa direttamente la password in chiaro
                    ConfirmPassword = passwordInChiaro, // Usa direttamente la password in chiaro
                    Nome = model.Nome,
                    Cognome = model.Cognome,
                    Cod_Fiscale = model.Cod_Fiscale,
                    Citta = model.Citta,
                    Prov = model.Prov,
                    Indirizzo = model.Indirizzo,
                    Tel_Cell = model.Tel_Cell,
                    email = model.Email,
                    
                };

                _context.USER.Add(user);

                try
                {
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Registrazione completata con successo. Ora puoi effettuare il login.";
                    return RedirectToAction("Index", "Home");
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}",
                                                                validationError.PropertyName,
                                                                validationError.ErrorMessage);
                        }
                    }
                    TempData["ErrorMessage"] = "Si è verificato un errore durante la registrazione. Controlla i log per maggiori dettagli.";
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Errore generico durante il salvataggio: {ex.Message}");
                    TempData["ErrorMessage"] = "Si è verificato un errore durante la registrazione.";
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorMessage"] = "Alcuni campi non sono validi. Si prega di correggerli e riprovare.";
            return RedirectToAction("Index", "Home");
        }

    }
}


