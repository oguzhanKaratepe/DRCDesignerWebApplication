using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DRCDesigner.DataAccess.Concrete
{
   public class SubdomainVersionReferenceRepository:Repository<SubdomainVersionReference>,ISubdomainVersionReferenceRepository
    {
        public SubdomainVersionReferenceRepository(DbContext context) : base(context)
        {
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
        public async Task<IList<SubdomainVersionReference>> getAllVersionReferences(int versionId)
        {
            return await DrcCardContext.SubdomainVersionReferences.Where(z => z.SubdomainVersionId == versionId).Include(c=>c.ReferencedSubdomainVersion).ToListAsync();
        }
        public IList<SubdomainVersionReference> getVersionReferencedSubdomainVersions(int versionId)
        {
            return DrcCardContext.SubdomainVersionReferences.Where(z => z.ReferencedVersionId == versionId).ToList();
        }
    }
}
