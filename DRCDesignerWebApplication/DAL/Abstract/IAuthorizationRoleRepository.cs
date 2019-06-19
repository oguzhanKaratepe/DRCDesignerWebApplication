using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface IAuthorizationRoleRepository: IRepository<AuthorizationRole>
    {
        IEnumerable<AuthorizationRole> getRolesByAuthorization(int id); //id:Authorization id
    }
}
