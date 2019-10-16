using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DRCDesigner.DataAccess.Concrete
{
    public class SubdomainVersionRepository :Repository<SubdomainVersion> ,ISubdomainVersionRepository
    {
        public SubdomainVersionRepository(DbContext context) : base(context)
        {
        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
        public async Task<IEnumerable<SubdomainVersion>> GetAllSubdomainVersionsBySubdomainId(int subdomainId)
        {
            return await DrcCardContext.SubdomainVersions.Where(m => m.SubdomainId == subdomainId).ToListAsync();
        }

        public async Task<SubdomainVersion> GetVersionWithReferencesById(int versionId)
        {
            return await DrcCardContext.SubdomainVersions.Where(m => m.Id == versionId)
                .Include(m => m.ReferencedSubdomainVersions).SingleOrDefaultAsync();
        }

        public async  Task<SubdomainVersion> GetSubdomainVersionCardsWithId(int versionId)
        {
            return await DrcCardContext.SubdomainVersions.Where(c => c.Id == versionId).Include(c => c.DRCards)
                .SingleAsync();
        }

        public async Task<bool> CheckIfSourceVersion(int versionId)
        {
            var sourceVersion = await DrcCardContext.SubdomainVersions.Where(m => m.SourceVersionId == versionId)
                .ToListAsync();
            if (sourceVersion.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public string getVersionNumber(int versionId)
        {
            return  DrcCardContext.SubdomainVersions.Where(m => m.Id == versionId).Select(m => m.VersionNumber).Single();
        }
    }
}
