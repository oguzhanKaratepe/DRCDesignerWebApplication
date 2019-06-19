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
    public class AuthorizationRoleRepository : Repository<AuthorizationRole>, IAuthorizationRoleRepository
    {
        public AuthorizationRoleRepository(DrcCardContext context) : base(context)
        {
        }

        public IEnumerable<AuthorizationRole> getRolesByAuthorization(int id) //Authorization id
        {
            return DrcCardContext.AuthorizationRoles.Where(s => s.AuthorizationId == id).ToList();
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }

     
    }
}
