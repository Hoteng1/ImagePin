using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> getAll();
        T getById(int id);
        void Create(T item);
        T Update(T item);
        void Delete(int id);
    }
}
