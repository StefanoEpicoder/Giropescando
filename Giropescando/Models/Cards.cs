using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Giropescando.Models
{
    public class Cards
    {
        [Key]
        public int IdCard { get; set; }
        public string Titolo { get; set; }
        public string Contenuto { get; set; }
        public string RaccontoCompleto { get; set; }
        public string MapUrl { get; set; }
        public string Immagine { get; set; }
    }
}
