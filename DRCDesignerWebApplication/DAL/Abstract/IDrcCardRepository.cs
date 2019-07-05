using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface IDrcCardRepository: IRepository<DrcCard>
    {
        DrcCard getDrcCardWithAllEntities(int id); //drcCard id
        int getDrcCardOrder(int id);
        void setDrcCardOrder(int id, int order);
        IEnumerable<DrcCard> getAllCardsBySubdomain(int subdomainId);

    }
}
