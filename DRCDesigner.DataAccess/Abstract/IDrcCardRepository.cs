
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
        string getDrcCardName(int id);
        IEnumerable<DrcCard> getAllCardsBySubdomainVersion(int subdomainVersionId);
        //void Update(DrcCard drcCard);
        //void Remove(DrcCard drcCard);
        IList<DrcCardResponsibility> GetDrcResponsibilities(int id); //drcCard id

        Task<DrcCard> GetByIdWithoutTracking(int id);

    }
}