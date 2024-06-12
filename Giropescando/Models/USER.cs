using Giropescando.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity.Spatial;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Giropescando.Models
{
    [Table("USER")]
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            Posts = new HashSet<Post>();
            Commenti = new HashSet<Commento>();
            MiPiace = new HashSet<MiPiace>();
        }

        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Pass { get; set; }

        public object PasswordHash { get; internal set; }
        public string ConfirmPassword { get; internal set; }

        [Required]
        [StringLength(50)]
        public string Ruolo { get; set; }

        public string Nome { get; set; }
        public string Cognome { get; set; }

        [Display(Name = "Codice Fiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Codice Fiscale incompleto")]
        [Remote("CFEsistente", "USER", ErrorMessage = "Cliente già inserito")]
        public string Cod_Fiscale { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }

        [StringLength(3, MinimumLength = 2, ErrorMessage = "Inserire Sigla Provincia")]
        [Display(Name = "Provincia")]
        public string Prov { get; set; }

        [Display(Name = "Indirizzo")]
        public string Indirizzo { get; set; }

        [Display(Name = "Telefono")]
        public string Tel_Cell { get; set; }

        [EmailAddress(ErrorMessage = "Inserire indirizzo e-mail valido")]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        // Relazioni con le altre tabelle
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Commento> Commenti { get; set; }
        public virtual ICollection<MiPiace> MiPiace { get; set; }






        //AUTENTICAZIONE
        public static bool Autenticato(string username, string password)
        {
            SqlConnection con = Connessioni.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM [USER] WHERE Username = @username and [Pass]=@pass", con);
                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar) { Value = username });
                cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.NVarChar) { Value = password });

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'autenticazione: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

    }
}