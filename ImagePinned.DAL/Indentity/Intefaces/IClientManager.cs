using ImagePinned.DAL.Indentity.Entietis;
using ImagePinned.DAL.Indentity.Indenties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.DAL.Indentity.Intefaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile item);
        void Delete(ClientProfile item);
    }
}
