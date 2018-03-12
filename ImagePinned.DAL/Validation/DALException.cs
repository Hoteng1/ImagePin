using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Validation
{
    public class DALException : Exception
    {
        public DALException() { }
        public DALException(string message) : base(message) { }
    }
}
