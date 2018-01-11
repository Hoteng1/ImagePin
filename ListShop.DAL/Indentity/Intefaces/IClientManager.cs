using ListShop.DAL.Indentity.Entietis;
using ListShop.DAL.Indentity.Indenties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Indentity.Intefaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile item);
        void Delete(ClientProfile item);
    }
}
