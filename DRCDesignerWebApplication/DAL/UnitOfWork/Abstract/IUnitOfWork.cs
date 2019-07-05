using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork
{
   public interface IUnitOfWork:IDisposable
    {
        int Complete();
    }
}
