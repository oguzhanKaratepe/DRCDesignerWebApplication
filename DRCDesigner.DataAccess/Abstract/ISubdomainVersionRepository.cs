using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface ISubdomainVersionRepository :IRepository<SubdomainVersion>
    {
        Task<IEnumerable<SubdomainVersion>> GetAllSubdomainVersionsBySubdomainId(int subdomainId);
        SubdomainVersion GetVersionWithReferencesById(int versionId);
        Task<SubdomainVersion> GetSubdomainVersionCardsWithId(int versionId);

        Task<bool> CheckIfSourceVersion(int versionId);
        string getVersionNumber(int versionId);

        

    }
}
