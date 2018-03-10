using ListShop.BLL.Identity.Interfaces;
using ListShop.DAL.Indentity.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagePinned.BLL.Interface;
using ImagePinned.BLL.Provider;

namespace ListShop.BLL.Identity.Service
{
    public class ServiceCreator : IServiceCreator
    {
        public IService CreateService(string connection)
        {
            return new Provider(new IdentityUnitOfWork(connection));
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        
    }
}
