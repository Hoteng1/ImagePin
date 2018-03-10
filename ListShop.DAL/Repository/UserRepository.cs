
using ImagePinned.DAL.Interfaces;
using ListShop.DAL.Entities;
using ListShop.DAL.Indentity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Repository
{
    public class UserRepository : IRepository<User>
    {
        ApplicationContext db;

        public UserRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public void Create(User item)
        {
           
            this.db.DbUsers.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = this.db.DbUsers.Find(id);
            if (user != null)
            {
                this.db.DbUsers.Remove(user);
                this.db.SaveChanges();
            }
           
        }

        public IEnumerable<User> getAll()
        {
            return this.db.DbUsers;
        }

        public User getById(int id)
        {
            return this.db.DbUsers.Find(id);
        }

        public User Update(User item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();
            return item;
        }
    }
}
