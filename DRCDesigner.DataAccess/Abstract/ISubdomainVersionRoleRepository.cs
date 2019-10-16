﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface ISubdomainVersionRoleRepository :IRepository<SubdomainVersionRole>
    {
        Task<IEnumerable<SubdomainVersionRole>> GetAllRoleVersionsByRoleId(int roleId);
      
        Task<IEnumerable<SubdomainVersionRole>> GetAllVersionRolesBySubdomainVersionId(int subdomainVersionId);
     
       
    }
}