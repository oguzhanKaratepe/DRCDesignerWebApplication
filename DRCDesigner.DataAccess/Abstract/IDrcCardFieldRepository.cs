using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;


namespace DRCDesigner.DataAccess.Abstract
{
    public interface IDrcCardFieldRepository : IRepository<DrcCardField>
    {
        List<DrcCardField> GetDrcCardFieldsByDrcCardId(int id);
        DrcCardField GetFieldCollaborationByFieldId(int id);
        List<DrcCardField> GetAllDrcCardFieldsByFieldId(int id);
        List<DrcCardField> GetDrcCardFieldCollaborationsByDrcCardId(int id);
        void RemoveDrcCardFieldCollaborationsByDrcCardId(int id);


    }
}
