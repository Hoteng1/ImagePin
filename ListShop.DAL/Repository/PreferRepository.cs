
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
    public class PreferRepository : IRepository<Prefer>
    {
        ApplicationContext db;

        public PreferRepository(ApplicationContext db)
        {
            this.db = db;
        }
        
        public void Create(Prefer item)
        {
            this.db.Prefers.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            Prefer prefer = this.db.Prefers.Find(id);
            if(prefer!=null)
            {
                this.db.Prefers.Remove(prefer);
                this.db.SaveChanges();
            }
        }

        public IEnumerable<Prefer> getAll()
        {
            return this.db.Prefers;
        }

        public Prefer getById(int id)
        {
            return this.db.Prefers.Find(id);
        }

        public Prefer Update(Prefer item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();
            return item;
        }
    }
}
