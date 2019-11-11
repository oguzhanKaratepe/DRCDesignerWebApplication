using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DRCDesigner.DataAccess.Concrete
{
    public class DrcCardFieldRepository : Repository<DrcCardField>, IDrcCardFieldRepository
    {
        public DrcCardFieldRepository(DrcCardContext context) : base(context)
        {

        }

        public DrcCardContext DrcCardContext
        {
            get { return _context as DrcCardContext; }
        }

        public List<DrcCardField> GetAllDrcCardFieldsByFieldId(int id)
        {
            return DrcCardContext.DrcCardFields.Where(a => a.FieldId == id).ToList();
        }
        public DrcCardField GetDrcCardIdByFieldId(int id)
        {
            return DrcCardContext.DrcCardFields.Single(a => a.FieldId == id && !a.IsRelationCollaboration);
        }
        public List<DrcCardField> GetDrcCardFieldCollaborationsByDrcCardId(int id)
        {
            return DrcCardContext.DrcCardFields.Where(a => a.DrcCardId == id && a.IsRelationCollaboration)
                .ToList();
        }

        public void RemoveDrcCardFieldCollaborationsByDrcCardId(int id)
        {
            var drcCardFields = DrcCardContext.DrcCardFields.Where(a => a.DrcCardId == id && a.IsRelationCollaboration)
                .ToList();
            if (drcCardFields != null)
            {
                foreach (var VARIABLE in drcCardFields)
                {
                    DrcCardContext.DrcCardFields.Remove(VARIABLE);
                }
            }
        }

        public List<DrcCardField> GetDrcCardFieldsByDrcCardId(int id)
        {
            return DrcCardContext.DrcCardFields.Where(a => a.DrcCardId == id && !a.IsRelationCollaboration)
                .ToList();
        }

        public DrcCardField GetFieldCollaborationByFieldId(int id)
        {
            return DrcCardContext.DrcCardFields.Where(a => a.FieldId == id && a.IsRelationCollaboration).SingleOrDefault();
        }
    }
}