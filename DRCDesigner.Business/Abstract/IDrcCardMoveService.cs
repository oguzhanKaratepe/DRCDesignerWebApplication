using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DRCDesigner.Business.BusinessModels;

namespace DRCDesigner.Business.Abstract
{
    public interface IDrcCardMoveService
    {
        Task<DocumentMoveObject> MoveCardToDestinationSubdomainAsync(int drcCardId, int targetSubdomainVersionId, string newDrcCardName);
        Task<string> CheckMoveOperationReferenceNeeds(int id, int subdomainVersionId);
    }
}
