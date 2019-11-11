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

        public IEnumerable<Field> getDrcCardAllFields(int id)
        {
            var result = DrcCardContext.Fields.Include(m => m.DrcCardFields).ThenInclude(m => m.DrcCard).ToList();
            return result.Where(m => m.DrcCardFields.Any(c => c.DrcCardId == id)).ToList();
        }
     
        public Field GetByIdWithoutTracking(int id)
        {
            return  DrcCardContext.Fields.AsNoTracking().Single(c => c.Id == id);
        }
    }

}
