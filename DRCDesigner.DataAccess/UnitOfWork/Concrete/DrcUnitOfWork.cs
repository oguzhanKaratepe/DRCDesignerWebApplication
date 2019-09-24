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
    public class DrcUnitOfWork :UnitOfWork, IDrcUnitOfWork
    {
        private DrcCardContext _context;
        public DrcUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;
            DrcCardRepository = new DrcCardRepository(_context);
            FieldRepository = new FieldRepository(_context);
            SubdomainRepository = new SubdomainRepository(_context);
            SubdomainVersionRepository= new SubdomainVersionRepository(_context);
            ResponsibilityRepository =new ResponsibilityRepository(_context);
            AuthorizationRepository=new AuthorizationRepository(_context);
            DrcCardResponsibilityRepository=new DrcCardResponsibilityRepository(_context);
            RoleRepository=new RoleRepository(_context);
            AuthorizationRoleRepository=new AuthorizationRoleRepository(_context);
            DrcCardFieldRepository=new DrcCardFieldRepository(_context);
        }

        public ISubdomainVersionRepository SubdomainVersionRepository { get; private set; }
        public IDrcCardRepository DrcCardRepository { get; private set; }
        
        public IFieldRepository FieldRepository { get; private set; }
        
        public ISubdomainRepository SubdomainRepository { get; private set; }
        public IResponsibilityRepository ResponsibilityRepository{ get; private set; }

        public IAuthorizationRepository AuthorizationRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public IAuthorizationRoleRepository AuthorizationRoleRepository { get; private set; }
        public IDrcCardResponsibilityRepository DrcCardResponsibilityRepository { get; private set; }
        public IDrcCardFieldRepository DrcCardFieldRepository { get; private set; }
    }
}
