using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.BLL.EntitesDTO
{
    public class ImageDTO
    {
        public string Title { get; set; }
        public int Id_pin { get; set; }
        public int Id_Ownd { get; set; }
        public int CountLike { get; set; }
        public byte[] Resurse { get; set; }
    }
}
