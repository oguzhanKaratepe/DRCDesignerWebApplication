
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;


namespace DRCDesigner.DataAccess.UnitOfWork.Abstract
{
   public interface IDocumentTransferUnitOfWork:IUnitOfWork
    {
        IDrcCardRepository DrcCardRepository { get;  }
        IDrcCardResponsibilityRepository DrcCardResponsibilityRepository { get; }
        IDrcCardFieldRepository DrcCardFieldRepository { get; }

    }
}
