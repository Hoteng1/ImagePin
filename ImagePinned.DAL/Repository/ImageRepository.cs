﻿
using ImagePinned.DAL.Interfaces;
using ImagePinned.DAL.Entities;
using ImagePinned.DAL.Indentity.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ImagePinned.DAL.Repository
{
    public class ImageRepository : IRepository<Image>
    {
        ApplicationContext db;

        public ImageRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public void Create(Image item)
        {
            db.Images.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            Image image = this.db.Images.Find(id);
            if (image != null)
            {
                this.db.Images.Remove(image);
                this.db.SaveChanges();
            }
        }

        public IEnumerable<Image> getAll()
        {

            return this.db.Images.ToList();

        }

        public Image getById(int id)
        {
            return this.db.Images.Find(id);
        }

        public Image Update(Image item)
        {

            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();

            return item;
        }
    }
}
