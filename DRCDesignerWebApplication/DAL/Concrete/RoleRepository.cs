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
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Role> getGlobalRoles()
        {

            return DrcCardContext.Roles.Where(s => s.SubdomainId ==null).ToList();
        }

        public IEnumerable<Role> getRolesBySubdomain(int id)
        {
            return DrcCardContext.Roles.Where(s => s.SubdomainId == id).ToList();
        }
        public async Task<IEnumerable<Role>> getRoles()
        {
            return await DrcCardContext.Roles.Include(s => s.Subdomain).ToListAsync();
        }
        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
