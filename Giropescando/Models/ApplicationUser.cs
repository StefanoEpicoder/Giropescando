using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Giropescando.Models;

public class ApplicationUser : IdentityUser
{
    public string Username { get; internal set; }
    public int IdUser { get; internal set; }

    // Aggiungi questa linea


    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
        // Nota che l'authenticationType deve corrispondere a quello definito in CookieAuthenticationOptions.AuthenticationType
        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        // Aggiungi qui i claim personalizzati dell'utente
        return userIdentity;
    }

    // Aggiungi ulteriori proprietà qui
}
