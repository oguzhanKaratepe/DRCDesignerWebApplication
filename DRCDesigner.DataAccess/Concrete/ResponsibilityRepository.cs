
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
        //public ICollection<Responsibility> GetResponsibilityWithDrcResponsibilities(int id)
        //{
        //    return DrcCardContext.Responsibilities.Where(m => m.Id == id).Include(m => m.DrcCardResponsibilities).ThenInclude(m=>m.IsRelationCollaboration)
        //        .ToList();
        //}
    }
}
