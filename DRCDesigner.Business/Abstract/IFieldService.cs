using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
   public interface IFieldService
   {
       void Add(Field field);
       void Update(Field field);
       void Delete(Field field);
   }
}
