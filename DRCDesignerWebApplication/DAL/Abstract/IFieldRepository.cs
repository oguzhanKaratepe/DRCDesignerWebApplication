using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
   public interface IFieldRepository: IRepository<Field>
    {
        IEnumerable<Field> getDrcCardAllFields(int id); //id: drcCard id

    }
}
