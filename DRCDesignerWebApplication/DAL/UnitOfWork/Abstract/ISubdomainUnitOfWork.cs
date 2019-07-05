using DRCDesignerWebApplication.DAL.Abstract;
using DRCDesignerWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.DAL.UnitOfWork.Abstract
{
    public interface ISubdomainUnitOfWork: IUnitOfWork
    { 
       ISubdomainRepository SubdomainRepository { get;  }
    }
}
