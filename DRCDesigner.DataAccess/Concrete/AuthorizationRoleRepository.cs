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
    public  class AuthorizationRoleRepository : Repository<AuthorizationRole>, IAuthorizationRoleRepository
    {
        public AuthorizationRoleRepository(DrcCardContext context) : base(context)
        {
        }
        public DrcCardContext DrcCardContext
        {
            get { return _context as DrcCardContext; }
        }

        public IList<AuthorizationRole> GetAuthorizationRolesByAuthorizationId(int id)
        {
            return DrcCardContext.AuthorizationRoles.Where(m => m.AuthorizationId == id).ToList();
        }

      public IList<AuthorizationRole> GetAuthorizationRolesByRoleId(int id)
      {
          return DrcCardContext.AuthorizationRoles.Where(m => m.RoleId == id).ToList();
      }
    }
}
