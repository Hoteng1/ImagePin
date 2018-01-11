using ListShop.DAL.Indentity.EF;
using ListShop.DAL.Indentity.Entietis;
using ListShop.DAL.Indentity.Intefaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Indentity.Repository
{
    public class ClientManager : IClientManager
    {
        public ApplicationContext Database { get; set; }
        public ClientManager(ApplicationContext db)
        {
            Database = db;
        }

        public void Delete(ClientProfile item)
        {
            Database.ClientProfiles.Attach(item);
            Database.Entry(item).State = EntityState.Deleted;
            Database.SaveChanges();
        }

        public void Create(ClientProfile item)
        {
            Database.ClientProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

       
    }
}
