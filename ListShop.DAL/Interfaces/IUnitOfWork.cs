using ListShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Pin> Pins { get; }
        IRepository<User> Users { get; }
        IRepository<Image> Images { get; }
        IRepository<Prefer> Prefers { get; }
        IRepository<Like> Likes { get; }
        void Save();
    }
}
