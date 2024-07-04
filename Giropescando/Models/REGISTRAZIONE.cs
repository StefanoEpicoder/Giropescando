using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giropescando.Models
{
    public class REGISTRAZIONE
    {
        [Required(ErrorMessage = "Il campo Username è obbligatorio.")]
        [StringLength(50, ErrorMessage = "La lunghezza dell'Username non può superare i 50 caratteri.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Il campo Conferma Password è obbligatorio.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La password e la conferma password non corrispondono.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Codice Fiscale è obbligatorio.")]
        public string Cod_Fiscale { get; set; }

        public string Citta { get; set; }

        public string Prov { get; set; }

        public string Indirizzo { get; set; }

        [Required(ErrorMessage = "Il campo Telefono Cellulare è obbligatorio.")]
        public string Tel_Cell { get; set; }

        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Formato email non valido.")]
        public string Email { get; set; }
    }
}