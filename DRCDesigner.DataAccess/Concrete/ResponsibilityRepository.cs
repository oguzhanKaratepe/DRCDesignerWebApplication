
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DRCDesigner.DataAccess.Concrete
{
    public class ResponsibilityRepository : Repository<Responsibility>, IResponsibilityRepository
    {
        public ResponsibilityRepository(DrcCardContext context) : base(context)
        {

        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
        public IEnumerable<Responsibility> GetDrcAllResponsibilities(int id)
        {
            var result = DrcCardContext.Responsibilities.Include(m => m.DrcCardResponsibilities).ThenInclude(m => m.DrcCard).ToList();

            return result.Where(m => m.DrcCardResponsibilities.Any(c => c.DrcCardId == id)).ToList();
        }
    
        public Responsibility GetByIdWithoutTracking(int id)
        {
            return  DrcCardContext.Responsibilities.AsNoTracking().Where(c => c.Id==id).Single();
        }
    }
}
