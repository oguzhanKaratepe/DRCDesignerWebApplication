
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Concrete
{
    public class DrcCardResponsibilityRepository : Repository<DrcCardResponsibility>, IDrcCardResponsibilityRepository
    {
        public DrcCardResponsibilityRepository(DrcCardContext context) : base(context)
        {
        }

        public DrcCardContext DrcCardContext
        {
            get { return _context as DrcCardContext; }
        }
        public IList<DrcCardResponsibility> GetAllDrcCardResponsibilitiesByDrcCardId(int id)
        {

            return DrcCardContext.DrcCardResponsibilities
                .Where(a => a.DrcCardId == id).ToList();
        }
        public IList<DrcCardResponsibility> GetDrcCardResponsibilitiesByDrcCardId(int id)
        {
            return DrcCardContext.DrcCardResponsibilities
                .Where(a => a.DrcCardId == id && !a.IsRelationCollaboration).ToList();
        }

        public IList<DrcCardResponsibility> GetShadowCardAllResponsibilityCollaborationsByDrcCardId(int id)
        {
            return DrcCardContext.DrcCardResponsibilities.Where(a => a.DrcCardId == id && a.IsRelationCollaboration).ToList();
        }

        public IList<DrcCardResponsibility> GetResponsibilityCollaborationsByResponsibilityId(int id)
        {
            return DrcCardContext.DrcCardResponsibilities
                .Where(a => a.ResponsibilityId == id && a.IsRelationCollaboration).ToList();
        }

        public IList<DrcCardResponsibility> GetResponsibilityAllRelationsByResponsibilityId(int id)
        {
            return DrcCardContext.DrcCardResponsibilities
                .Where(a => a.ResponsibilityId == id).ToList();
        }

        public void RemoveAllDrcCardResponsibilityCollaborationsByDrcCardId(int id)
        {
            var drcCardResCollaborations = DrcCardContext.DrcCardResponsibilities
                .Where(a => a.DrcCardId == id && a.IsRelationCollaboration).ToList();
            if (drcCardResCollaborations != null)
            {
                foreach (var VARIABLE in drcCardResCollaborations)
                {
                    DrcCardContext.DrcCardResponsibilities.Remove(VARIABLE);
                }
            }
        }
    }
}