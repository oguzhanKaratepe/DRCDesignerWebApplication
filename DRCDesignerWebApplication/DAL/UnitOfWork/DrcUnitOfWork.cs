using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Concrete;
using DRCDesignerWebApplication.DAL.Context;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class DrcUnitOfWork : IUnitOfWork
    {
        private DrcCardContext _context;
        public DrcUnitOfWork(DrcCardContext context)
        {
            _context = context;
            DrcCardRepository = new DrcCardRepository(_context);
            ResponsibilityCollaborationRepository = new ResponsibilityCollaborationRepository(_context);
            FieldRepository = new FieldRepository(_context);
            AuthorizationRoleRepository = new AuthorizationRoleRepository(_context);
        }

        public IDrcCardRepository DrcCardRepository { get; private set; }
        public IResponsibilityCollaborationRepository ResponsibilityCollaborationRepository { get; private set; }
        public IFieldRepository FieldRepository { get; private set; }
        public IAuthorizationRoleRepository AuthorizationRoleRepository { get; private set; }
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
