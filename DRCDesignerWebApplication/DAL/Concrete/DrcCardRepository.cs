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
            return DrcCardContext.DrcCards.Where(s => s.Id == id).Single().Order;
        }

        public void setDrcCardOrder(int id, int order) //drcCard id and Card order
        {
            DrcCard DrcCard = DrcCardContext.DrcCards.Where(s => s.Id == id).Single();
            DrcCard.Order = order;
            DrcCardContext.SaveChanges();
        }

        public DrcCardContext DrcCardContext { get { return _context as DrcCardContext; } }
    }
}
