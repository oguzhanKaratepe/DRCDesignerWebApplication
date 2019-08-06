using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
    public class SubdomainUnitOfWork : DAL.UnitOfWork.Concrete.UnitOfWork, ISubdomainUnitOfWork
    {
        private DrcCardContext _context;
        public SubdomainUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;

           
            SubdomainRepository = new SubdomainRepository(_context);


        }
     
        public ISubdomainRepository SubdomainRepository { get; private set; }


    }
}
