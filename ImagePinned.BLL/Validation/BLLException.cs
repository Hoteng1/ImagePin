using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.BLL.Validation
{
    public class BLLException : Exception
    {
        public BLLException() { }
        public BLLException(string message) : base(message) { }
    }
}
