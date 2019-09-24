
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface IAuthorizationRepository:IRepository<Authorization>
    {
       Task<IList<Authorization>> GetAuthorizationsByDrcCardId(int Id);
       Task<Authorization> GetByIdWithoutTracking(int id);
    }
}
