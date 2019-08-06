using DRCDesignerWebApplication.DAL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork.Abstract
{
   public interface IDrcUnitOfWork:IUnitOfWork
    {
        ISubdomainRepository SubdomainRepository { get; }
        IDrcCardRepository DrcCardRepository { get;  }
    
        IFieldRepository FieldRepository { get;}

    }
}
