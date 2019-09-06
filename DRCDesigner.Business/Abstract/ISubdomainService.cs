using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Abstract
{
    public interface ISubdomainService
    {
        Task<IEnumerable<Subdomain>> GetAll();
        Task<IEnumerable<Subdomain>> GetMoveDropDownBoxSubdomains(int subdomainId);
        void Add(string values);
        void Update(string values, int id);
        Task<bool> Remove(int subdomainId);

    }
}
