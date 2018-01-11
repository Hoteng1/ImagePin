using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ListShop.Web.Models
{
    public class ImageView
    {
        [Display(Name ="Название")]
        [Required]
        public string Title { get; set; }
        [Display(Name = "Пин")]
        [Required]
        public string Pin { get; set; }
        public byte[] Resurse { get; set; }
    }
}