using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Giropescando.Models
{
    public class Ruoli : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        // Metodo aggiornato per restituire sempre tutti i ruoli necessari
        public override string[] GetRolesForUser(string username)
        {
            using (var db = new ModelDbContext()) // Assumi di avere una classe di contesto del database
            {
                // Trova l'utente nel database tramite username
                var user = db.USER.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    // Restituisce il ruolo dell'utente in un array di stringhe
                    return new string[] { user.Ruolo };
                }
            }

            // Se l'utente non viene trovato o non ha un ruolo, restituisce un array vuoto
            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new ModelDbContext())
            {
                var user = db.USER.FirstOrDefault(u => u.Username == username);
                return user != null && user.Ruolo.Equals(roleName, StringComparison.OrdinalIgnoreCase);
            }
        }


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
