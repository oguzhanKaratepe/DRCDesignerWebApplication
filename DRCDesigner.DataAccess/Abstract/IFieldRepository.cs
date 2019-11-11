
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
   public interface IFieldRepository: IRepository<Field>
    {
        IEnumerable<Field> getDrcCardAllFields(int id); //id: drcCard id

       Field GetByIdWithoutTracking(int id);
    }
}
