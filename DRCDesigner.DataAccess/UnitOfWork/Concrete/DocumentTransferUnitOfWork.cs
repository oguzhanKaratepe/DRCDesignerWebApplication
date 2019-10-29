using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;


namespace DRCDesigner.DataAccess.UnitOfWork.Concrete
{
    public class DocumentTransferUnitOfWork :UnitOfWork, IDocumentTransferUnitOfWork
    {
        private DrcCardContext _context;
        public DocumentTransferUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;
            DrcCardRepository = new DrcCardRepository(_context);
            DrcCardResponsibilityRepository=new DrcCardResponsibilityRepository(_context);
            DrcCardFieldRepository=new DrcCardFieldRepository(_context);
            SubdomainVersionReferenceRepository=new SubdomainVersionReferenceRepository(_context);
            SubdomainVersionRepository=new SubdomainVersionRepository(_context);
            SubdomainRepository=new SubdomainRepository(_context);
            FieldRepository=new FieldRepository(_context);
            ResponsibilityRepository=new ResponsibilityRepository(_context);
            AuthorizationRepository=new AuthorizationRepository(_context);
            AuthorizationRoleRepository=new AuthorizationRoleRepository(_context);
        }
       public ISubdomainRepository SubdomainRepository { get; private set; }
        public IDrcCardRepository DrcCardRepository { get; private set; }
        public IDrcCardResponsibilityRepository DrcCardResponsibilityRepository { get; private set; }
        public IDrcCardFieldRepository DrcCardFieldRepository { get; private set; }
        public ISubdomainVersionRepository SubdomainVersionRepository { get; private set; }
        public ISubdomainVersionReferenceRepository SubdomainVersionReferenceRepository { get; private set; }
        public IFieldRepository FieldRepository { get; private set; }
       public IResponsibilityRepository ResponsibilityRepository { get; private set; }
       public IAuthorizationRepository AuthorizationRepository { get; private set; }
       public IAuthorizationRoleRepository AuthorizationRoleRepository { get; private set; }
    }
}
