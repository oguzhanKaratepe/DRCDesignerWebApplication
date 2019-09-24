using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
   public interface ISubdomainVersionService
    {
       
        Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllSubdomainVersionSourceOptions(int id,int subdomainId);
        Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllSubdomainVersions(int subdomainId);
        Task<IEnumerable<SubdomainVersionBusinessModel>> GetAllVersions();
        Task<IList<SubdomainVersionBusinessModel>> GetReferenceOptions(int subdomainId);
       
        //Task<IEnumerable<Subdomain>> GetMoveDropDownBoxSubdomains(int subdomainId);
        Task<bool> Add(string values);
       void Update(string values, int id);
        Task<bool> LookForSourceChange(int id,string values);
        Task<bool> Remove(int subdomainVersionId);
    }
}
