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
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
