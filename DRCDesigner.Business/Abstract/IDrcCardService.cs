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
       Task<String> Delete(int id);
       IList<ResponsibilityBusinessModel> getListOfDrcCardResponsibilities(int cardId);
       IList<FieldBusinessModel> getListOfDrcCardFields(int cardId,int? mainCardId);
       IList<AuthorizationBusinessModel> getListOfDrcCardAuthorizations(int cardId);
        Task<IList<DrcCardBusinessModel>> GetAllDrcCards(int subdomainVersionId);
        Task<IEnumerable<SubdomainMenuItemBusinessModel>> GetAllSubdomainMenuItems(int versionId);
        Task<IList<ShadowCardSelectBoxBusinessModel>> GetShadowSelectBoxOptions(int subdomainVersionId);

        string GetPresentationHeader(int versionId);
        string GetShadowCardSourcePath(int? shadowCardId);
       DrcCardBusinessModel GetCard(int id);
       int TotalSubdomainSize();
       bool isSubdomainVersionLocked(int id);
       Task<IList<DrcCard>> GetCardCollaborationOptions(int cardId);

       int getVersionIdFromQueryString(String subdomain,String version);
   }
}
