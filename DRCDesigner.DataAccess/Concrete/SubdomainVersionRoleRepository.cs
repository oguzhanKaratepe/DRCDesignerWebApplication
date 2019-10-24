
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
    public class SubdomainVersionRoleRepository : Repository<SubdomainVersionRole>, ISubdomainVersionRoleRepository
    {
        public SubdomainVersionRoleRepository(DbContext context) : base(context)
        {
        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }

        public async  Task<IEnumerable<SubdomainVersionRole>> GetAllRoleVersionsByRoleId(int roleId)
        {
            return  await  DrcCardContext.SubdomainVersionRoles.Where(c => c.RoleId == roleId).ToListAsync();
        }


        public async Task<IEnumerable<SubdomainVersionRole>> GetAllVersionRolesBySubdomainVersionId(int subdomainVersionId)
        {
            return await DrcCardContext.SubdomainVersionRoles.AsNoTracking().Where(c => c.SubdomainVersionId == subdomainVersionId).ToListAsync();
        }

       
    }


}