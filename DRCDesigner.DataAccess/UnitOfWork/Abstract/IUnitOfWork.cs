using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesigner.DataAccess.UnitOfWork.Abstract
{
   public interface IUnitOfWork:IDisposable
    {
        int Complete();
    }
}
