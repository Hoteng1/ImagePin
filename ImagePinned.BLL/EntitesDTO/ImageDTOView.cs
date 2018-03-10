using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.BLL.EntitesDTO
{
    public class ImageDTOView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Pin { get; set; }
        public string Own_Name { get; set; }
        public int CountLike { get; set; }
        public byte[] Resurse { get; set; }
    }
}
