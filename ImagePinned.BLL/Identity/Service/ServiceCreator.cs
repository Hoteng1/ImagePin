using ImagePinned.BLL.Identity.Interfaces;
using ImagePinned.BLL.Interface;
using ImagePinned.DAL.Indentity.Repository;

namespace ImagePinned.BLL.Identity.Service
{
    public class ServiceCreator : IServiceCreator
    {
        public IService CreateService(string connection)
        {
            return new Provider.Provider(new IdentityUnitOfWork(connection));
        }

        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        
    }
}
