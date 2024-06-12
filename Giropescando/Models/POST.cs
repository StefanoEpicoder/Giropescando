using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Giropescando.Models
{
    [Table("POST")]
    public class Post
    {
        [Key]
        public int IdPost { get; set; }

        [Required]
        public string Titolo { get; set; } // Nuovo campo per il titolo del post

        [Required]
        public string Contenuto { get; set; }

        [Required]
    
        public DateTime DataPubblicazione { get; set; }

        [Required]
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual USER User { get; set; }

        public byte[] Immagine { get; set; } // Dati dell'immagine

        public virtual ICollection<Commento> Commenti { get; set; }
        public virtual ICollection<MiPiace> MiPiace { get; set; }

        // Proprietà di sola lettura che restituisce il nome dell'utente associato al post
        [NotMapped] // Questa annotazione impedisce a Entity Framework di mappare questa proprietà su una colonna del database
        public string NomeAutore
        {
            get
            {
                return User?.Username; // Assumendo che l'entità USER abbia una proprietà Username
            }
        }
    }

}
