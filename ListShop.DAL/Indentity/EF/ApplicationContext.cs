using ListShop.DAL.Entities;
using ListShop.DAL.Indentity.Entietis;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Indentity.EF
{
    public class ApplicationContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }
        public ApplicationContext() : base() { }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<User> DbUsers { get; set; }
        public DbSet<Pin> Pins { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Prefer> Prefers { get; set; }
        public DbSet<Like> Likes { get; set; }
    }
}
