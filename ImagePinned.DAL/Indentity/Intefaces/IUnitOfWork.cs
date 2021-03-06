﻿using ImagePinned.DAL.Interfaces;
using ImagePinned.DAL.Entities;
using ImagePinned.DAL.Indentity.Indenties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagePinned.DAL.Indentity.Intefaces
{
    public interface IUnitOfWork:IDisposable
    {
        ApplicationUserManager UserManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IClientManager ClientManager { get; }
        IRepository<Pin> Pins { get; }
        IRepository<User> Users { get; }
        IRepository<Image> Images { get; }
        IRepository<Prefer> Prefers { get; }
        IRepository<Like> Likes { get; }
        void Save();
        Task SaveAsync();
    }
}
