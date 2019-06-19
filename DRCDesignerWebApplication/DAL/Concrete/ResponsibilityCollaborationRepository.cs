using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Concrete
{
    public class ResponsibilityCollaborationRepository : Repository<ResponsibilityCollaboration>, IResponsibilityCollaborationRepository
    {
        public ResponsibilityCollaborationRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<ResponsibilityCollaboration> getDrcCardAllResponsibilityCollaboration(int id) //drc id
        {

            return DrcCardContext.ResponsibilityCollaborations.Where(s => s.DrcCardId == id).ToList();
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
