using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Concrete;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class RoleUnitOfWork : IUnitOfWork
    {
        private DrcCardContext _context;
        public RoleUnitOfWork(DrcCardContext context)
        {
            _context = context;
            SubdomainRepository = new Repository<Subdomain>(_context);
            RoleRepository = new RoleRepository(_context);
        }
        public IRepository<Subdomain> SubdomainRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
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
