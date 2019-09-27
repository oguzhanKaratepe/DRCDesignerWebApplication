using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;

namespace DRCDesigner.Business.Abstract
{
    public interface IAuthorizationService
    {
        Task<IList<AuthorizationBusinessModel>> GetCardAuthorizations(int cardId);
     
        void Add(string values);
        void Update(int id, string values);
        void Delete(int id);
        Task<IEnumerable<object>> GetAuthorizationRoles(int id);
    }
}
