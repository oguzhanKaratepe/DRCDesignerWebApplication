﻿
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;

namespace DRCDesigner.DataAccess.UnitOfWork.Concrete
{
    public class SubdomainUnitOfWork :UnitOfWork, ISubdomainUnitOfWork
    {
        private DrcCardContext _context;
        public SubdomainUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;
            SubdomainRepository = new SubdomainRepository(_context);
            DrcCardRepository=new DrcCardRepository(_context);
            SubdomainVersionRepository=new SubdomainVersionRepository(_context);
            SubdomainVersionReferenceRepository=new SubdomainVersionReferenceRepository(_context);
            SubdomainVersionRoleRepository=new SubdomainVersionRoleRepository(_context);
            RoleRepository=new RoleRepository(_context);
        }
     
        public ISubdomainRepository SubdomainRepository { get; private set; }
        public ISubdomainVersionRepository SubdomainVersionRepository { get; private set;}
        public ISubdomainVersionReferenceRepository SubdomainVersionReferenceRepository { get; }
        public IDrcCardRepository DrcCardRepository { get; private set; }
        public ISubdomainVersionRoleRepository SubdomainVersionRoleRepository { get; private set; }
        public IRoleRepository RoleRepository { get; }
    }
}
