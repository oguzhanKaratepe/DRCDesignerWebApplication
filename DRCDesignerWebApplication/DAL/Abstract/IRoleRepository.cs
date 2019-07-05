using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
   public interface IRoleRepository:IRepository<Role>
    {
        IEnumerable<Role> getRolesBySubdomain(int id); //id:subdomain id
        IEnumerable<Role> getGlobalRoles();
         Task<IEnumerable<Role>> getRoles();
    }
}
