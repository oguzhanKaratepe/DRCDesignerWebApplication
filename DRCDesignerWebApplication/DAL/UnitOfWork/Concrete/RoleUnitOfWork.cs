using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Concrete;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.DAL.UnitOfWork.Concrete;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class RoleUnitOfWork:UnitOfWork.Concrete.UnitOfWork, IRoleUnitOfWork
    {
        private DrcCardContext _context;
        public RoleUnitOfWork(DrcCardContext context):base(context)
        {
            _context = context;
            SubdomainRepository = new Repository<Subdomain>(_context);
            RoleRepository = new RoleRepository(_context);
        }
        public IRepository<Subdomain> SubdomainRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
       
    }
}
