
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Concrete
{
    public class SubdomainRepository : Repository<Subdomain>, ISubdomainRepository

    {
        public SubdomainRepository(DrcCardContext context) : base(context)
        {

        }

        public int subdomainSize()
        {
            return DrcCardContext.Subdomains.Count();
        }

        public async Task<IEnumerable<Subdomain>> GetAllWithVersions()
        {

            return await DrcCardContext.Subdomains.Include(m => m.SubdomainVersions).ToListAsync();
        }

        public async Task<Subdomain> GetSubdomainWithAllVersions(int subdomainId)
        {
            return await DrcCardContext.Subdomains.Where(m => m.Id == subdomainId).Include(n => n.SubdomainVersions)
                .SingleAsync();
        }

        public string GetSubdomainName(int subdomainId)
        {
            return DrcCardContext.Subdomains.Where(m => m.Id == subdomainId).Select(m => m.SubdomainName).SingleOrDefault();
        }

        public string GetSubdomainNamespace(int subdomainId)
        {
            return DrcCardContext.Subdomains.Where(m => m.Id == subdomainId).Select(m => m.SubdomainNamespace).SingleOrDefault();
        }

        public Subdomain GetSubdomainWithAllVersionsWithSubdomainName(string subdomainName)
        {
            return DrcCardContext.Subdomains.Where(m => m.SubdomainName.ToLower().Replace(" ","").Equals(subdomainName.ToLower())).Include(n=>n.SubdomainVersions).SingleOrDefault();
        }


        //public void Remove(Subdomain subdomain)
        //{

        //    var existingSubdomain = DrcCardContext.Subdomains
        //        .Where(p => p.Id == subdomain.Id)
        //        .Include(p => p.DRCards)
        //        .SingleOrDefault();



        //    if (existingSubdomain != null)
        //    {
        //        foreach (var existingCard in existingSubdomain.DRCards)
        //        {
        //            if (!subdomain.DRCards.Any(c => c.Id == existingCard.Id))
        //            {

        //            }

        //        }
        //    }

        //}



        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}