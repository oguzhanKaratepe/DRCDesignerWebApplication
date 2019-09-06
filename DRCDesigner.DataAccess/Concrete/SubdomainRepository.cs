
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