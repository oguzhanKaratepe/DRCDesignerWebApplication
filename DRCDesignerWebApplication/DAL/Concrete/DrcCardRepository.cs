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
    public class DrcCardRepository : Repository<DrcCard>, IDrcCardRepository
    {
        public DrcCardRepository(DrcCardContext context) : base(context)
        {
        }
       
        public int getDrcCardOrder(int id) //drcCard id
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();

            return DrcCard.Order;
        }
        public DrcCard getDrcCardWithAllEntities(int id) //drcCard id
        {
            
            return DrcCardContext.DrcCards.Include(a=>a.Fields).Include(a => a.Responsibilities).Include(a=>a.Authorizations).Where(s => s.Id == id).Single();
            
        }


        public void setDrcCardOrder(int id, int order) //drcCard id and Card order
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();
            DrcCard.Order = order;
            DrcCardContext.SaveChanges();
        }

        public IEnumerable<DrcCard> getAllCardsBySubdomain(int subdomainId)
        {
            var Subdomaincards = DrcCardContext.DrcCards.Where(s => s.SubdomainId == subdomainId).ToList();

            return Subdomaincards;
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
