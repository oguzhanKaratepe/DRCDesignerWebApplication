using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
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


        public async Task<string> CheckMoveOperationReferenceNeeds(int drcCardId, int targetSubdomainVersionId)
        {
            string checkDocumentRef = await checkDocumentCollaboration(drcCardId, targetSubdomainVersionId);
            if (String.IsNullOrEmpty(checkDocumentRef))
            {
                return "";
            }
            else
            {
                var version = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(targetSubdomainVersionId);
                var SubdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(version.SubdomainId);

                return "You have some collaboration documents and your destination " + SubdomainName + ":" + version.VersionNumber + " does not have the fallowing subdomanin versions as Reference." + checkDocumentRef;
            }

        }

        private async Task<bool> checkRefSourceToDestination(int source, int destination)
        {
            var targetVersionReferences = await _documentTransferUnitOfWork.SubdomainVersionReferenceRepository.getAllVersionReferences(destination);
            foreach (var targetVersionReference in targetVersionReferences)
            {
                if (source == targetVersionReference.ReferencedVersionId)
                {
                    return false;
                }
            }

            return true;
        }
        private async Task<string> checkDocumentCollaboration(int drcCardId, int targetId)
        {
            string returnString = "";
            var drcDocumentResponsibilities = _documentTransferUnitOfWork.ResponsibilityRepository.GetDrcAllResponsibilities(drcCardId);
            foreach (var drcDocumentResponsibility in drcDocumentResponsibilities)
            {
                var collaborations = _documentTransferUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(drcDocumentResponsibility.Id);
                foreach (var collaboration in collaborations)
                {
                    var collaborationDocument = _documentTransferUnitOfWork.DrcCardRepository.GetById(collaboration.DrcCardId);
                    if (collaborationDocument.MainCardId != null)
                    {
                        var sourceDocument = _documentTransferUnitOfWork.DrcCardRepository.GetById((int)collaborationDocument.MainCardId);
                        bool refNeed = await checkRefSourceToDestination(sourceDocument.SubdomainVersionId, targetId);
                        if (refNeed)
                        {
                            var sourceVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(sourceDocument.SubdomainVersionId);
                            var subdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(sourceVersion.SubdomainId);
                            returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + sourceDocument.DrcCardName + ", ";
                        }
                    }
                    else
                    {
                        bool refNeed = await checkRefSourceToDestination(collaborationDocument.SubdomainVersionId, targetId);
                        if (refNeed)
                        {
                            var sourceVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(collaborationDocument.SubdomainVersionId);
                            var subdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(sourceVersion.SubdomainId);
                            returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + collaborationDocument.DrcCardName +", ";
                        }
                    }
                }
            }
            var drcDocumentFields = _documentTransferUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);
            foreach (var drcDocumentField in drcDocumentFields)
            {
                var collaboration = _documentTransferUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(drcDocumentField.Id);

                var collaborationDocument = _documentTransferUnitOfWork.DrcCardRepository.GetById(collaboration.DrcCardId);
                if (collaborationDocument.MainCardId != null)
                {
                    var sourceDocument = _documentTransferUnitOfWork.DrcCardRepository.GetById((int)collaborationDocument.MainCardId);

                    bool refNeed = await checkRefSourceToDestination(sourceDocument.SubdomainVersionId, targetId);
                    if (refNeed)
                    {
                        var sourceVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(sourceDocument.SubdomainVersionId);
                        var subdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(sourceVersion.SubdomainId);
                        returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + sourceDocument.DrcCardName +", ";
                    }

                }
                else
                {
                    bool refNeed = await checkRefSourceToDestination(collaborationDocument.SubdomainVersionId, targetId);
                    if (refNeed)
                    {
                        var sourceVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(collaborationDocument.SubdomainVersionId);
                        var subdomainName = _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(sourceVersion.SubdomainId);
                        returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + collaborationDocument.DrcCardName +", ";
                    }
                }
            }

            return returnString;
        }


        public async Task<DocumentMoveObject> MoveCardToDestinationSubdomainAsync(int drcCardId, int targetSubdomainVersionId, string newDrcCardName)
        {
            DocumentMoveObject moveObject = new DocumentMoveObject
            {
                DrcCardIdToMove = drcCardId,
                TargetSubdomainVersionId = targetSubdomainVersionId,
                NewDocumentName = newDrcCardName
            };

            var ifConnectedToAnyOtherDocument = checkIfDocumentConnectedToCurrentVersion(drcCardId);

            if (!String.IsNullOrWhiteSpace(ifConnectedToAnyOtherDocument))
            {
                moveObject.MoveResultType = MoveResultType.DocumentHasConnections;
                moveObject.MoveResultDefinition = ifConnectedToAnyOtherDocument;
                return moveObject;
            }

            var moveAction = true; // await MoveDocumentAction(drcCardId, targetSubdomainVersionId, newDrcCardName);

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


        private bool MoveDocumentWithoutAnyRelation(int drcCardId, int targetSubdomainVersionId, string newDrcCardName)
        {
            try
            {
                DrcCard document = _documentTransferUnitOfWork.DrcCardRepository.GetById(drcCardId);
                document.SubdomainVersionId = targetSubdomainVersionId;
                document.DrcCardName = newDrcCardName;
                _documentTransferUnitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            

        }


        private async Task<Collection<DrcCard>> getCollaborationDocuments(int drcCardId)
        {
            var drcDocumentResponsibilities = _documentTransferUnitOfWork.ResponsibilityRepository.GetDrcAllResponsibilities(drcCardId);
            Collection<DrcCard> cardsToNeedToCreateShadow = new Collection<DrcCard>();

            foreach (var drcDocumentResponsibility in drcDocumentResponsibilities)
            {
                var collaborations = _documentTransferUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(drcDocumentResponsibility.Id);
                foreach (var collaboration in collaborations)
                {
                    var drcCard = await _documentTransferUnitOfWork.DrcCardRepository.GetByIdWithoutTracking(collaboration.DrcCardId);
                    cardsToNeedToCreateShadow.Add(drcCard);
                }
            }
            var drcDocumentFields = _documentTransferUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);
            foreach (var drcDocumentField in drcDocumentFields)
            {
                var collaboration = _documentTransferUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(drcDocumentField.Id);
                if (collaboration != null)
                {
                    var drcCard = await _documentTransferUnitOfWork.DrcCardRepository.GetByIdWithoutTracking(collaboration.DrcCardId);
                    cardsToNeedToCreateShadow.Add(drcCard);
                }
            }
            return cardsToNeedToCreateShadow;
        }

        private bool createShadows(Collection<DrcCard> drcCards, int targetVersion)
        {
            //foreach (var card in drcCards)
            //{
            //    if (card.MainCardId != null)
            //    {

            //    }
            //}


            //var targetVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(targetVersion);

            //bool needToCreateShadow = true;
            //foreach (var drcDocument in targetVersion.DRCards)
            //{
            //    if (drcDocument.MainCardId == oldId || drcDocument.Id == oldId)
            //    {
            //        needToCreateShadow = false;
            //    }

            //}

            //if (needToCreateShadow)
            //{
            //    DrcCard newShadowCard = new DrcCard();
            //    newShadowCard = drcCard;
            //    newShadowCard.Id = 0;
            //    newShadowCard.SubdomainVersionId = targetSubdomainVersionId;
            //    newShadowCard.MainCardId = oldId;
            //    _documentTransferUnitOfWork.DrcCardRepository.Add(newShadowCard);

            //}
            //_documentTransferUnitOfWork.Complete();

            //return null;
            return true;
        }
        private String checkIfDocumentConnectedToCurrentVersion(int drcCardId)
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
    }

}
