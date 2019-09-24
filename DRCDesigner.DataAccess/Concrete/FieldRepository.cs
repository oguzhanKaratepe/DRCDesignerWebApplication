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
    public class FieldRepository : Repository<Field>, IFieldRepository 
    {
        public FieldRepository(DrcCardContext context) : base(context)
        {

        }

   
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }

        //public IEnumerable<Field> getDrcCardAllFields(int id)
        //{
        //    return DrcCardContext.Fields.Where(m => m.DrcCardId == id).ToList();
        //}
        public async Task<Field> GetByIdWithoutTracking(int id)
        {
            return await DrcCardContext.Fields.AsNoTracking().Where(c => c.Id == id).SingleAsync();
        }
    }

}
