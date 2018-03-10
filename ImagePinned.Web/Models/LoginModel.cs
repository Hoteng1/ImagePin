using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImagePinned.Web.Models
{
    public class LoginModel
    {
        [Display(Name = "E-mail")]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Пороль")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}