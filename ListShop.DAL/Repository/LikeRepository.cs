
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
    public class LikeRepository : IRepository<Like>
    {
        ApplicationContext db;

        public LikeRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public void Create(Like item)
        {
            this.db.Likes.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            Like like = this.db.Likes.Find(id);
            if(like!=null)
            {
                this.db.Likes.Remove(like);
                this.db.SaveChanges();
            }
        }

        public IEnumerable<Like> getAll()
        {
            return this.db.Likes;
        }

        public Like getById(int id)
        {
            return this.db.Likes.Find(id);
        }

        public Like Update(Like item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();

            return item;
        }
    }
}
