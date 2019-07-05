using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DRCDesignerWebApplication.DAL.Concrete
{
    public class FieldRepository : Repository<Field>, IFieldRepository 
    {
        public FieldRepository(DrcCardContext context) : base(context)
        {

        }

        public ICollection<Field> getDrcCardAllFields(int id) //drcCard id
        {
            return DrcCardContext.Fields.Where(s => s.drcCardId == id).ToList();
        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
