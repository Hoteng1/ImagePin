﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ListShop.Web.Models
{
    public class UserModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Role { get; set; }
    }
}