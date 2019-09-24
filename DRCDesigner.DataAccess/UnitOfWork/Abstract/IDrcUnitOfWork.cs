
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;


namespace DRCDesigner.DataAccess.UnitOfWork.Abstract
{
   public interface IDrcUnitOfWork:IUnitOfWork
    {
        ISubdomainRepository SubdomainRepository { get; }
        ISubdomainVersionRepository SubdomainVersionRepository { get; }
        IDrcCardRepository DrcCardRepository { get;  }
        IFieldRepository FieldRepository { get;}
        IResponsibilityRepository ResponsibilityRepository { get; }
        IAuthorizationRepository AuthorizationRepository { get; }
        IRoleRepository RoleRepository { get; }
        IAuthorizationRoleRepository AuthorizationRoleRepository { get; }
        IDrcCardResponsibilityRepository DrcCardResponsibilityRepository { get; }
        IDrcCardFieldRepository DrcCardFieldRepository { get; }


    }
}
