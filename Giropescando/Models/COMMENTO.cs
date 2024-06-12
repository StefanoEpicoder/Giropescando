using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Giropescando.Models
{
    [Table("COMMENTO")]
    public class Commento
    {
        [Key]
        public int IdCommento { get; set; }

        [Required]
        public string Contenuto { get; set; }

        [Required]
        public DateTime DataPubblicazione { get; set; }

        [Required]
        public int IdUser { get; set; }

        [ForeignKey("IdUser")]
        public virtual USER User { get; set; }

        [Required]
        public int IdPost { get; set; }

        [ForeignKey("IdPost")]
        public virtual Post Post { get; set; }
    }
}