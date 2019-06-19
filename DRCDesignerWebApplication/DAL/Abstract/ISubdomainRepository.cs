using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface ISubdomainRepository :IRepository<Subdomain>
    {
       IEnumerable<Subdomain> getProjectSubdomains(int id); //project id
       
    }
}
