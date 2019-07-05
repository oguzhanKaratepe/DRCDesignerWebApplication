using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Concrete
{
    public class ResponsibilityRepository : Repository<Responsibility>,IResponsibilityRepository
    {
        public ResponsibilityRepository(DrcCardContext context) : base(context)
        {

        }
        public ICollection<Responsibility> getDrcCardAllResponsibilities(int id)
        {
            throw new NotImplementedException();
        }
    }
}
