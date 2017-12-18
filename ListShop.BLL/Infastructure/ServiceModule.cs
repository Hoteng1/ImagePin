using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using ImagePinned.DAL.Interfaces;
using ImagePinned.DAL.Repository;

namespace ImagePinned.BLL.Infastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;

        public ServiceModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EfUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
