
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Core;
using DRCDesigner.Core.DataAccess;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.DataAccess.Abstract
{
    public interface IDrcCardRepository : IRepository<DrcCard>
    {
        DrcCard getDrcCardWithAllEntities(int id); //drcCard id
        int getDrcCardOrder(int id);
        void setDrcCardOrder(int id, int order);
        Task<IEnumerable<DrcCard>> getAllCardsBySubdomain(int SubdomainID);
        //void Update(DrcCard drcCard);
        //void Remove(DrcCard drcCard);
        IList<DrcCardResponsibility> GetDrcResponsibilities(int id); //drcCard id



    }
}