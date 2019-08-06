using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Concrete;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class DrcUnitOfWork : UnitOfWork.Concrete.UnitOfWork, IDrcUnitOfWork
    {
        private DrcCardContext _context;
        public DrcUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;

            DrcCardRepository = new DrcCardRepository(_context);
          
            FieldRepository = new FieldRepository(_context);
            AuthorizationRoleRepository = new AuthorizationRoleRepository(_context);
            SubdomainRepository = new SubdomainRepository(_context);
        }

        public IDrcCardRepository DrcCardRepository { get; private set; }
        
        public IFieldRepository FieldRepository { get; private set; }
        public IAuthorizationRoleRepository AuthorizationRoleRepository { get; private set; }

        public ISubdomainRepository SubdomainRepository { get; private set; }
    }
}
