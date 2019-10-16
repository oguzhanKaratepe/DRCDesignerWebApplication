
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;

namespace DRCDesigner.DataAccess.UnitOfWork.Concrete
{
    public class RoleUnitOfWork: UnitOfWork, IRoleUnitOfWork
    {
        private DrcCardContext _context;
        private IRoleUnitOfWork _roleUnitOfWorkImplementation;

        public RoleUnitOfWork(DrcCardContext context):base(context)
        {
            _context = context;
            SubdomainRepository = new SubdomainRepository(_context);
            RoleRepository = new RoleRepository(_context);
            AuthorizationRepository = new AuthorizationRepository(_context);
            AuthorizationRoleRepository = new AuthorizationRoleRepository(_context);
            SubdomainVersionRoleRepository=new SubdomainVersionRoleRepository(_context);
            SubdomainVersionRepository=new SubdomainVersionRepository(_context);
        }
        public ISubdomainRepository SubdomainRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public ISubdomainVersionRoleRepository SubdomainVersionRoleRepository { get; private set; }
        public ISubdomainVersionRepository SubdomainVersionRepository { get; private set; }

        public IAuthorizationRepository AuthorizationRepository { get; private set; }
        public IAuthorizationRoleRepository AuthorizationRoleRepository { get; private set; }
    }

}
