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
       Task<IList<DrcCard>> GetResponsibilityShadows(int versionId, int cardId);
        Task<IList<ResponsibilityBusinessModel>> GetCardResponsibilities(int cardId);
        void Add(string values);
       void Update(int id,string values);
       void Delete(int id);
   }
}
