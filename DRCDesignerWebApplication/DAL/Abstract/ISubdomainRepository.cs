using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface ISubdomainRepository :IRepository<Subdomain>
    {
        int subdomainSize();
       
        Task <ICollection<Subdomain>> searchSubdomainName( string searchString);
    }
}
