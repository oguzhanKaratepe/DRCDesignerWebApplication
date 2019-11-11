using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
    public interface ISubdomainService
    {
        IEnumerable<Subdomain> GetAll();
        Task<IEnumerable<SubdomainVersion>> GetAllSubdomainVersions(int subdomainId);
       Task<IEnumerable<SubdomainVersionBusinessModel>> GetMoveDropDownBoxSubdomains(int subdomainVersionId);
       void Add(string values);
        void Update(string values, int id);
        Task<bool> Remove(int subdomainId);

    }
}
