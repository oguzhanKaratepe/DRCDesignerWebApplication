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
       Task<IList<DrcCard>> GetCollaborations(int versionId, int cardId);
       Task<IList<FieldBusinessModel>> GetCardFields(int cardId);
       void Add(string values);
       void Update(int id,string values);
       void Delete(int id);
       
   }
}
