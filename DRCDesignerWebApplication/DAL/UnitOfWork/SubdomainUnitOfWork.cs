using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class SubdomainUnitOfWork : IUnitOfWork
    {
        private DrcCardContext _context;
        public SubdomainUnitOfWork(DrcCardContext context)
        {
            _context = context;
            ProjectRepository = new Repository<DrcProject>(_context);
            SubdomainRepository = new Repository<Subdomain>(_context);


        }
        public IRepository<DrcProject> ProjectRepository { get; private set; }
        public IRepository<Subdomain> SubdomainRepository { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
