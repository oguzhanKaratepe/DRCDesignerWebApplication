
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Concrete
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Role> getGlobalRoles()
        {
            throw new NotImplementedException();
           // return DrcCardContext.Roles.Where(s => s.SubdomainVersionId==null).ToList();
        }

        public IEnumerable<Role> getRolesBySubdomain(int id)
        {
            //    return DrcCardContext.Roles.Where(s => s.SubdomainId == id).ToList();
            throw new NotImplementedException();
        }
 

        public async Task<IEnumerable<Role>> getRoles()
        {
            return await DrcCardContext.Roles.Include(s => s.SubdomainVersion).ToListAsync();
        }

        public async Task<ICollection<Role>> searchRoleName(string searchString)
        {
            return await DrcCardContext.Roles.Where(s => s.RoleName.ToLower().Contains(searchString)).ToListAsync();
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
