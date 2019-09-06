using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
   public interface IDrcCardService
   {
       void Add(DrcCard drcCard);
       void Update(DrcCard drcCard);
       void Delete(DrcCard drcCard);
       Task<IList<ShadowCardSelectBoxBusinessModel>> GetShadowSelectBoxOptions(int id);
       bool MoveCardToDestinationSubdomain(int destId, int cardId);
       string GetShadowCardSourcePath(int? shadowId);
   }
}
