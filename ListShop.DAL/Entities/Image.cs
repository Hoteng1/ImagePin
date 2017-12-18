using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListShop.DAL.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Id_pin { get; set; }
        public int Id_Ownd { get; set; }
        public int CountLike { get; set; }
        public byte[] Resurse { get; set; }
    }
}
