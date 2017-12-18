using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListShop.Web.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Pin { get; set; }
        public string Own_Name { get; set; }
        public int CountLike { get; set; }
        public byte[] Resurse { get; set; } 

    }
}