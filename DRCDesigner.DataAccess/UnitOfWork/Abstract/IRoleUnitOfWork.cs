
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;

namespace DRCDesigner.DataAccess.UnitOfWork.Abstract
{
   public interface IRoleUnitOfWork:IUnitOfWork
    {
        ISubdomainRepository SubdomainRepository { get;  }
        IRoleRepository RoleRepository { get;  }

        ISubdomainVersionRoleRepository SubdomainVersionRoleRepository { get; }
        ISubdomainVersionRepository SubdomainVersionRepository { get; }
        IAuthorizationRepository AuthorizationRepository { get; }
        IAuthorizationRoleRepository AuthorizationRoleRepository { get; }
    }
}
