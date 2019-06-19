using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.Abstract
{
    public interface IDrcCardRepository: IRepository<DrcCard>
    {
        int getDrcCardOrder(int id);
        void setDrcCardOrder(int id, int order);

    }
}
