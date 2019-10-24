
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

       Task<IEnumerable<Role>> getGlobalRoles();

   }
}
