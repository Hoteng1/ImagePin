﻿using ImagePinned.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.BLL.Identity.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
        IService CreateService(string connection);
    }
}
