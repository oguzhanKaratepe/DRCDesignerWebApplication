using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface ISubdomainRepository :IRepository<Subdomain>
    {
        int subdomainSize();
        void Remove(Subdomain subdomain);
        Task<IEnumerable<Subdomain>> GetAllWithVersions();
        Task<Subdomain> GetSubdomainWithAllVersions(int subdomainId);
        string GetSubdomainName(int subdomainId);
    }
}
