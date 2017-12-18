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
    class PinRepository : IRepository<Pin>
    {
        PinContext db;

        public PinRepository(PinContext db)
        {
            this.db = db;
        }

        public void Create(Pin item)
        {
            this.db.Pins.Add(item);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            Pin pin=this.db.Pins.Find(id);
            if(pin!=null)
            {
                this.db.Pins.Remove(pin);
                this.db.SaveChanges();
            }
        }

        public IEnumerable<Pin> getAll()
        {
            return this.db.Pins;
        }

        public Pin getById(int id)
        {
            return this.db.Pins.Find(id);
        }

        public Pin Update(Pin item)
        {
            db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            this.db.SaveChanges();
            return item;
        }
    }
}
