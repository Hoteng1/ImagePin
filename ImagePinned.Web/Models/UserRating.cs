using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImagePinned.Web.Models
{
    public class UserRating
    {

        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Count_Like { get; set; }
    }
}