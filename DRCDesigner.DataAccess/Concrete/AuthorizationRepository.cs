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
using Authorization = DRCDesigner.Entities.Concrete.Authorization;

namespace DRCDesigner.DataAccess.Concrete
{
    public class AuthorizationRepository :Repository<Entities.Concrete.Authorization>,IAuthorizationRepository
    {
        public AuthorizationRepository(DrcCardContext context) : base(context)
        {

        }
        public IList<Entities.Concrete.Authorization> GetAuthorizationsByDrcCardId(int Id)
        {
            return  DrcCardContext.Authorizations.Include(m => m.AuthorizationRoles).Where(m => m.DrcCardId == Id).ToList();
        }

        public Authorization GetByIdWithoutTracking(int id)
        {
           return  DrcCardContext.Authorizations.AsNoTracking().Single(c => c.Id==id);
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
