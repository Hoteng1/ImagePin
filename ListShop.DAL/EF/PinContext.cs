using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListShop.DAL.Entities;

namespace ImagePinned.DAL.EF
{
    public class PinContext:DbContext
    {
        
        public DbSet<User> Users { get; set; }
        public DbSet<Pin> Pins { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Prefer> Prefers { get; set; }
        public DbSet<Like> Likes { get; set; }

        public PinContext(string connectionString) : base(connectionString)
        {

        }
    }
}
