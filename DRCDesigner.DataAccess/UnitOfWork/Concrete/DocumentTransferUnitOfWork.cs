using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.Abstract;
using DRCDesigner.DataAccess.Concrete;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;


namespace DRCDesigner.DataAccess.UnitOfWork.Concrete
{
    public class DocumentTransferUnitOfWork :UnitOfWork, IDocumentTransferUnitOfWork
    {
        private DrcCardContext _context;
        public DocumentTransferUnitOfWork(DrcCardContext context) : base(context)
        {
            _context = context;
            DrcCardRepository = new DrcCardRepository(_context);
            DrcCardResponsibilityRepository=new DrcCardResponsibilityRepository(_context);
            DrcCardFieldRepository=new DrcCardFieldRepository(_context);
        }

        public IDrcCardRepository DrcCardRepository { get; private set; }
        public IDrcCardResponsibilityRepository DrcCardResponsibilityRepository { get; private set; }
        public IDrcCardFieldRepository DrcCardFieldRepository { get; private set; }
    }
}
