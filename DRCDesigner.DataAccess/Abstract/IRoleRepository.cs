﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
   public interface IRoleRepository:IRepository<Role>
    {
        IEnumerable<Role> getRolesBySubdomain(int id); //id:subdomain id
        IEnumerable<Role> getGlobalRoles();
         Task<IEnumerable<Role>> getRoles();
        Task<ICollection<Role>> searchRoleName(string searchString);
     
    }
}