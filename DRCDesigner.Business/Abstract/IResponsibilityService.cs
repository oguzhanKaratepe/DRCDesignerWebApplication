using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
   public interface IResponsibilityService
   {
       void Add(Responsibility responsibility);
       void Update(Responsibility responsibility);
       void Delete(Responsibility responsibility);
   }
}
