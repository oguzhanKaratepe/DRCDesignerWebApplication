using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL
{
    public class SubdomainRepository : Repository<Subdomain>, ISubdomainRepository

    {
        public SubdomainRepository(DrcCardContext context) : base(context)
        {
            
        }
    

        public async Task<ICollection<Subdomain>> searchSubdomainName(String searchString)
        {
            return await DrcCardContext.Subdomains.Where(s => s.SubdomainName.ToLower().Contains(searchString.ToLower())).ToListAsync();
        }
        public int subdomainSize()
        {
            return DrcCardContext.Subdomains.Count();
        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}