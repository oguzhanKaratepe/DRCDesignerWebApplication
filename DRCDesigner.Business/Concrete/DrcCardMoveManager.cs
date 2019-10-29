using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Concrete
{
    public class DrcCardMoveManager : IDrcCardMoveService
    {

     
        private IDocumentTransferUnitOfWork _documentTransferUnitOfWork;
        private IMapper _mapper;
        public DrcCardMoveManager(IMapper mapper, IDrcUnitOfWork drcUnitOfWork, IDocumentTransferUnitOfWork documentTransferUnitOfWork)
        {
            _documentTransferUnitOfWork = documentTransferUnitOfWork;
            _mapper = mapper;
        }


        
        public async Task<DocumentMoveObject> MoveCardToDestinationSubdomainAsync(int drcCardId, int targetSubdomainVersionId, string newDrcCardName)
        {
            DocumentMoveObject moveObject=new DocumentMoveObject
            {
                DrcCardIdToMove = drcCardId,
                TargetSubdomainVersionId = targetSubdomainVersionId,
                NewDocumentName = newDrcCardName

            };
            
            var ifConnectedToAnyOtherDocument=checkIfDocumentConnectedToCurrentVersion(drcCardId);

            if (!String.IsNullOrWhiteSpace(ifConnectedToAnyOtherDocument))
            {
                moveObject.MoveResultType = MoveResultType.DocumentHasConnections;
                moveObject.MoveResultDefinition = ifConnectedToAnyOtherDocument;
                return moveObject;
            }

            var referenceNeed = await checkIfReferenceNeeded(drcCardId, targetSubdomainVersionId);
            if (!String.IsNullOrEmpty(referenceNeed))
            {
                moveObject.MoveResultType = MoveResultType.TargetVersionReferenceProblem;
                moveObject.MoveResultDefinition = referenceNeed;
                return moveObject;
            }
            
            var moveAction=MoveDocumentAction(drcCardId, targetSubdomainVersionId, newDrcCardName);

            if (moveAction)
            {
                moveObject.MoveResultType = MoveResultType.Success;
                moveObject.MoveResultDefinition = "Success";
                return moveObject;
            }

            moveObject.MoveResultType = MoveResultType.Fail;
            moveObject.MoveResultDefinition = "Fail error";
            return moveObject;
            
        }

        private async Task<string> checkIfReferenceNeeded(int drcCardId, int targetSubdomainVersionId)
        {
            bool checkDocument =checkDocumentCollaboration(drcCardId);

            if (!checkDocument)
            {
                return "";
            }

            var version = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(targetSubdomainVersionId);
            var SubdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(version.SubdomainId);

            var targetVersionReferences = await _documentTransferUnitOfWork.SubdomainVersionReferenceRepository.getAllVersionReferences(
                    targetSubdomainVersionId);

            var drcCard= _documentTransferUnitOfWork.DrcCardRepository.GetById(drcCardId);
            foreach (var targetVersionReference in targetVersionReferences)
            {
                if (drcCard.SubdomainVersionId == targetVersionReference.ReferencedVersionId)
                {
                    return "";
                }
            }

            return "You have some collaboration documents and your destination "+SubdomainName+":"+version.VersionNumber+" does not have the current subdomanin version as Reference. Please add current version to destination version as reference. After that you can move your documents with its collaborations." ;
        }

        private bool checkDocumentCollaboration(int drcCardId)
        {
            var drcDocumentResponsibilities = _documentTransferUnitOfWork.ResponsibilityRepository.GetDrcAllResponsibilities(drcCardId);
            foreach (var drcDocumentResponsibility in drcDocumentResponsibilities)
            {
                var collaborations =
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository
                        .GetResponsibilityCollaborationsByResponsibilityId(drcDocumentResponsibility.Id);
                if (collaborations.Count > 0)
                {
                    return true;
                    
                }
            }
            var drcDocumentFields = _documentTransferUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);
            foreach (var drcDocumentField in drcDocumentFields)
            {
                var collaborations =
                    _documentTransferUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(drcDocumentField
                        .Id);
                if (collaborations !=null)
                {
                    return true;
                }
            }
           
            return false;
        }

        public String checkIfDocumentConnectedToCurrentVersion(int drcCardId)
        {
            String startSentence = "To be able to move this document you must first delete relations with fallowing documents: ";
            String connectedDocuments = "";
            var fieldConnections = _documentTransferUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldCollaborationsByDrcCardId(drcCardId);
            var responsibilityConnections = _documentTransferUnitOfWork.DrcCardResponsibilityRepository.GetShadowCardAllResponsibilityCollaborationsByDrcCardId(drcCardId);
            int added = -1;
            foreach (var fieldConnection in fieldConnections)
            {
               
                if (added != fieldConnection.DrcCardId)
                {
                    var connectionDrcCardField = _documentTransferUnitOfWork.DrcCardFieldRepository.GetDrcCardIdByFieldId(fieldConnection.FieldId);
                    added = fieldConnection.DrcCardId;
                    connectedDocuments += _documentTransferUnitOfWork.DrcCardRepository.getDrcCardName(connectionDrcCardField.DrcCardId) + " (by Field) ";
                }
              
            }
            added = -1;
            foreach (var resConnection in responsibilityConnections)
            {
                if (added != resConnection.DrcCardId)
                {
                    var connectionDrcCardRes = _documentTransferUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilityByResponsibilityId(resConnection.ResponsibilityId);
                connectedDocuments += _documentTransferUnitOfWork.DrcCardRepository.getDrcCardName(connectionDrcCardRes.DrcCardId) + " (by Responsibility) ";
                added = resConnection.DrcCardId;
                }
            }

            if (!String.IsNullOrWhiteSpace(connectedDocuments))
            {
                connectedDocuments = startSentence + connectedDocuments;
            }

            _documentTransferUnitOfWork.Complete();
            return connectedDocuments;
        }


        public bool MoveDocumentAction(int drcCardId, int targetSubdomainVersionId, string newDrcCardName)
        {
            //var collaborationsMove=CreateCollaborationDocumentsShadowsToTargetVersion(drcCardId,targetSubdomainVersionId);

            //var documentToMove = _documentTransferUnitOfWork.DrcCardRepository.GetById(drcCardId);
            //_documentTransferUnitOfWork.DrcCardRepository.Remove(documentToMove);
            //documentToMove.Id = 0;
            //documentToMove.SubdomainVersionId = targetSubdomainVersionId;
            //_documentTransferUnitOfWork.DrcCardRepository.Add(documentToMove);
            //var documentResponsibilities =_documentTransferUnitOfWork.ResponsibilityRepository.GetDrcAllResponsibilities(drcCardId);
            //var s =
            //    _documentTransferUnitOfWork.DrcCardResponsibilityRepository
            //        .GetAllDrcCardResponsibilitiesByDrcCardId(drcCardId);
            //_documentTransferUnitOfWork.Complete();
            return true;
        }


        private async Task<bool> CreateCollaborationDocumentsShadowsToTargetVersion(int drcCardId, int targetSubdomainVersionId)
        {
            var drcDocumentResponsibilities = _documentTransferUnitOfWork.ResponsibilityRepository.GetDrcAllResponsibilities(drcCardId);
            foreach (var drcDocumentResponsibility in drcDocumentResponsibilities)
            {
                var collaborations = _documentTransferUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(drcDocumentResponsibility.Id);
                foreach (var collaboration  in collaborations)
                {
                    var drcCard = await _documentTransferUnitOfWork.DrcCardRepository.GetByIdWithoutTracking(collaboration.DrcCardId);
                    createShadow(drcCard,targetSubdomainVersionId);
                }
              
            }
            var drcDocumentFields = _documentTransferUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);
            foreach (var drcDocumentField in drcDocumentFields)
            {
                var collaboration = _documentTransferUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(drcDocumentField.Id);
                if(collaboration !=null)
                {
                    var drcCard = await _documentTransferUnitOfWork.DrcCardRepository.GetByIdWithoutTracking(collaboration.DrcCardId);
                    createShadow(drcCard, targetSubdomainVersionId);
                }
            }

            return true;
        }

        private  async void createShadow(DrcCard drcCard, int targetSubdomainVersionId)
        {
            int oldId = drcCard.Id;

            if (drcCard.MainCardId != null)
            {
                oldId = (int)drcCard.MainCardId;
            }

            var targetVersion =
                await _documentTransferUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(
                    targetSubdomainVersionId);
            bool needToCreateShadow = true;
            foreach (var drcDocument in targetVersion.DRCards)
            {
                if (drcDocument.MainCardId == oldId ||drcDocument.Id==oldId)
                {
                    needToCreateShadow = false;
                }

            }

            if (needToCreateShadow)
            {
                DrcCard newShadowCard = new DrcCard();
                newShadowCard = drcCard;
                newShadowCard.Id = 0;
                newShadowCard.SubdomainVersionId = targetSubdomainVersionId;
                newShadowCard.MainCardId = oldId;
                _documentTransferUnitOfWork.DrcCardRepository.Add(newShadowCard);
                
            }
            _documentTransferUnitOfWork.Complete();
        }
    }

}
