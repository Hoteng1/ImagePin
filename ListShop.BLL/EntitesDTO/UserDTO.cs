using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.BLL.EntitesDTO
{
    public class UserDTO
    {
        public int Id_user { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Role { get; set; }
    }
}
