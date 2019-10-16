
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;

namespace DRCDesigner.DataAccess.UnitOfWork.Abstract
{
    public interface ISubdomainUnitOfWork: IUnitOfWork
    { 
       ISubdomainRepository SubdomainRepository { get;  }
       IDrcCardRepository DrcCardRepository { get; }
       ISubdomainVersionRepository SubdomainVersionRepository { get; }
       ISubdomainVersionReferenceRepository SubdomainVersionReferenceRepository { get; }
       ISubdomainVersionRoleRepository SubdomainVersionRoleRepository { get; }
       IRoleRepository RoleRepository { get; }
    }
}
