using DRCDesigner.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Net;
using DRCDesigner.Core.DataAccess.EntityFrameworkCore;

namespace DRCDesigner.DataAccess.Concrete
{
    public class AuthorizationRepository :Repository<Entities.Concrete.Authorization>,IAuthorizationRepository
    {
        public AuthorizationRepository(DrcCardContext context) : base(context)
        {

        }
        public async Task<IList<Entities.Concrete.Authorization>> GetAuthorizationsByDrcCardId(int Id)
        {
            return await DrcCardContext.Authorizations.Include(m => m.AuthorizationRoles).Where(m => m.DrcCardId == Id).ToListAsync();
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
