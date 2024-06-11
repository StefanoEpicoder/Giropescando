using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Giropescando.Models
{
    public class ModelDbContext : IdentityDbContext<ApplicationUser>
    {
        public ModelDbContext() : base("ModelDBcontext")
        {
        }

        public DbSet<Cards> Cards { get; set; }
        public DbSet<USER> USER { get; set; }


        public DbSet<IdentityUserLogin> IdentityUserLogins { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRoles { get; set; }

    }

}

