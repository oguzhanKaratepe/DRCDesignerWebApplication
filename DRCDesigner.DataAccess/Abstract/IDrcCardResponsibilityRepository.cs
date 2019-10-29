
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface IDrcCardResponsibilityRepository : IRepository<DrcCardResponsibility>
    {
        IList<DrcCardResponsibility> GetAllDrcCardResponsibilitiesByDrcCardId(int id);
        IList<DrcCardResponsibility> GetDrcCardResponsibilitiesByDrcCardId(int id);
        IList<DrcCardResponsibility> GetShadowCardAllResponsibilityCollaborationsByDrcCardId(int id);
        IList<DrcCardResponsibility> GetResponsibilityCollaborationsByResponsibilityId(int id);
        IList<DrcCardResponsibility> GetDrcCardResponsibilitiesByResponsibilityId(int id);
        void RemoveAllDrcCardResponsibilityCollaborationsByDrcCardId(int id);
        DrcCardResponsibility GetDrcCardResponsibilityByResponsibilityId(int id);

    }
}