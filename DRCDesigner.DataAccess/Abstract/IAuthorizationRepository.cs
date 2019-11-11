
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
       IList<Authorization> GetAuthorizationsByDrcCardId(int Id);
       Authorization GetByIdWithoutTracking(int id);
    }
}
