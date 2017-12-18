using ListShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.Web.OfWokUser
{
    public interface IRepository
    {
        UserModel Login(string userName, string password);
        UserModel GetUser(string name);
    }
}
