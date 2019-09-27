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
       void Add(DrcCardBusinessModel drcCard);
       void AddShadowCard(DrcCard drcCard);
       void Update(int id,string values);
       void Delete(DrcCard drcCard);
       Task<IList<ResponsibilityBusinessModel>> getListOfDrcCardResponsibilities(int cardId);
       Task<IList<FieldBusinessModel>> getListOfDrcCardFields(int cardId);
       Task<IList<AuthorizationBusinessModel>> getListOfDrcCardAuthorizations(int cardId);
        Task<IList<DrcCardBusinessModel>> GetAllDrcCards(int subdomainVersionId);
        Task<IEnumerable<SubdomainMenuItemBusinessModel>> GetAllSubdomainMenuItems(int versionId);
        Task<IList<ShadowCardSelectBoxBusinessModel>> GetShadowSelectBoxOptions(int subdomainVersionId);
       bool MoveCardToDestinationSubdomain(DrcCard drcCard);
       string GetShadowCardSourcePath(int? shadowCardId);
       DrcCardBusinessModel GetCard(int id);
       int TotalSubdomainSize();
       Task<IList<DrcCard>> GetCardCollaborationOptions(int Id, int cardId);
   }
}
