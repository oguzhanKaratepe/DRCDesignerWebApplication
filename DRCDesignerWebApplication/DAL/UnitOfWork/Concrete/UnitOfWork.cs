using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        protected DbContext _context;
        public UnitOfWork(DbContext context)
        {
            _context = context;

        }
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
