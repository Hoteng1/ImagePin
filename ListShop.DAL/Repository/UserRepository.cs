using ImagePinned.DAL.EF;
using ImagePinned.DAL.Interfaces;
using ListShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        PinContext db;

        public UserRepository(PinContext db)
        {
            this.db = db;
        }

        public void Create(User item)
        {
           
            this.db.Users.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = this.db.Users.Find(id);
            if (user != null)
            {
                this.db.Users.Remove(user);
                this.db.SaveChanges();
            }
           
        }

        public IEnumerable<User> getAll()
        {
            return this.db.Users;
        }

        public User getById(int id)
        {
            return this.db.Users.Find(id);
        }

        public User Update(User item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();
            return item;
        }
    }
}
