using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            try
            {
                string checkDocumentRef = await checkDocumentCollaboration(drcCardId, targetSubdomainVersionId);
                if (String.IsNullOrEmpty(checkDocumentRef))
                {
                    return "";
                }
                else
                {

                    var version =
                        _documentTransferUnitOfWork.SubdomainVersionRepository.GetById(targetSubdomainVersionId);
                    var SubdomainName =
                        _documentTransferUnitOfWork.SubdomainRepository.GetSubdomainName(version.SubdomainId);

                    return "You have some collaboration documents and your destination " + SubdomainName + ":" +
                           version.VersionNumber + " does not have the fallowing subdomanin versions as Reference." +
                           checkDocumentRef;


                }
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }

        private async Task<bool> checkRefSourceToDestination(int source, int destination)
        {
            var targetVersionReferences = await _documentTransferUnitOfWork.SubdomainVersionReferenceRepository.getAllVersionReferences(destination);
            foreach (var targetVersionReference in targetVersionReferences)
            {
                if (source == targetVersionReference.ReferencedVersionId || source == destination)
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
                            returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + collaborationDocument.DrcCardName + ", ";
                        }
                    }
                }
            }
            var drcDocumentFields = _documentTransferUnitOfWork.FieldRepository.getDrcCardAllFields(drcCardId);
            foreach (var drcDocumentField in drcDocumentFields)
            {
                var collaboration = _documentTransferUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(drcDocumentField.Id);

                if (collaboration != null)
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
                            returnString += subdomainName + ": " + sourceVersion.VersionNumber + ": " + collaborationDocument.DrcCardName + ", ";
                        }
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

                moveObject.MoveResultDefinition = "To be able to move this document you must first delete relations with fallowing documents: " + ifConnectedToAnyOtherDocument;
                return moveObject;
            }

            var checkForDublicate = checkForDublicateDocument(drcCardId, targetSubdomainVersionId);

            if (!String.IsNullOrWhiteSpace(checkForDublicate))
            {
                moveObject.MoveResultType = MoveResultType.Fail;
                moveObject.MoveResultDefinition = checkForDublicate;
                return moveObject;
            }

            var moveAction = MoveDocument(drcCardId, targetSubdomainVersionId, newDrcCardName);

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

        private string checkForDublicateDocument(int drcCardId, int targetSubdomainVersionId)
        {
            var version = _documentTransferUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(targetSubdomainVersionId);
            var movingCard = _documentTransferUnitOfWork.DrcCardRepository.GetById(drcCardId);
            foreach (var targetCard in version.DRCards.ToList())
            {
                //orijinal to shadow
                if (targetCard.MainCardId != null && targetCard.MainCardId == drcCardId)
                {
                    return "You are trying to move " + movingCard.DrcCardName + " but it has a shadow document: ( " + targetCard.DrcCardName + " ) on target version. You are not allowed to keep shadow and it's orijin card inside same subdomain version. Please copy your required shadow card instance parts to orijinal card then remove shadow";
                }
                //shadow to shadow
                else if (movingCard.MainCardId != null && targetCard.MainCardId != null && targetCard.MainCardId == movingCard.MainCardId)
                {
                    return "You are trying to move a shadow document "+movingCard.DrcCardName+" but in your target version you already have same shadow with "+targetCard.DrcCardName+" name. You are not allowed to keep 2 shadow card from same orijinal card inside same subdomain version. Please first delete target version shadow or copy your required card instance parts to target shadow card.";
                }
                //shadow to original
                else if (movingCard.MainCardId != null && movingCard.MainCardId == targetCard.Id)
                {
                    return "You are trying to move a shadow document " + movingCard.DrcCardName + " but in your target version you already orijin of this shadow document. You are not allowed to keep shadow and it's orijin card inside same subdomain version. Please copy your required shadow card instance parts to target orijinal card.";
                }
            }

            return "";
        }

        private bool MoveDocument(int drcCardId, int targetSubdomainVersionId, string newDrcCardName)
        {
            try
            {
                List<DrcCard> collaborationCards = getCollaborationCardsofACard(drcCardId);
                createShadows(collaborationCards, targetSubdomainVersionId); //this will create shadows

                DrcCard document = _documentTransferUnitOfWork.DrcCardRepository.GetById(drcCardId);

                document.SubdomainVersionId = targetSubdomainVersionId;
                document.DrcCardName = newDrcCardName;
                updateDrcCardCollaborationsAfterMove(drcCardId, targetSubdomainVersionId);
                _documentTransferUnitOfWork.Complete();

                removeUnusedCollaborationsAfterMove(collaborationCards);
                _documentTransferUnitOfWork.Complete();
                return true;
            }
            catch (Exception e)
            {
                return false;

            }
        }

        private void removeUnusedCollaborationsAfterMove(List<DrcCard> collaborationCards)
        {

            foreach (var collaborationCard in collaborationCards)
            {
                if (collaborationCard.MainCardId != null)
                {
                    bool delete = true;

                    var card = _documentTransferUnitOfWork.DrcCardRepository.getDrcCardWithAllEntities(collaborationCard.Id);

                    foreach (var authorization in card.Authorizations)
                    {
                        if (authorization.AuthorizationRoles.Count > 0)
                        {
                            delete = false;
                        }
                    }

                    if (card.DrcCardFields.Count > 0 || card.DrcCardResponsibilities.Count > 0)
                    {
                        delete = false;
                    }

                    if (delete)
                    {
                        _documentTransferUnitOfWork.DrcCardRepository.Remove(collaborationCard);
                    }
                }
            }
        }


        private void updateDrcCardCollaborationsAfterMove(int drcCardId, int versionId)
        {
            var version = _documentTransferUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(versionId);
            var drcCard = _documentTransferUnitOfWork.DrcCardRepository.getDrcCardWithAllEntities(drcCardId);

            foreach (var res in drcCard.DrcCardResponsibilities.ToList())
            {
                foreach (var resCol in res.Responsibility.DrcCardResponsibilities.ToList())
                {
                    if (resCol.IsRelationCollaboration)
                    {
                        reArrangeResCollaborationsAfterMove(version.DRCards, resCol);
                    }
                }
            }
            foreach (var field in drcCard.DrcCardFields.ToList())
            {
                foreach (var fieldCol in field.Field.DrcCardFields.ToList())
                {
                    if (fieldCol.IsRelationCollaboration)
                    {
                        reArrangeFieldCollaborationsAfterMove(version.DRCards, fieldCol);

                    }
                }

            }

        }

        private void reArrangeResCollaborationsAfterMove(ICollection<DrcCard> targetVersionCards, DrcCardResponsibility resCollaboration)
        {
            var collaborationCard = _documentTransferUnitOfWork.DrcCardRepository.GetById(resCollaboration.DrcCardId);
            foreach (var drcCard in targetVersionCards.ToList())
            {
                if (drcCard.MainCardId != null && collaborationCard.MainCardId != null && (collaborationCard.MainCardId == (int)drcCard.MainCardId))
                {

                    DrcCardResponsibility newResponsibility = new DrcCardResponsibility();
                    newResponsibility.Responsibility = resCollaboration.Responsibility;
                    newResponsibility.ResponsibilityId = resCollaboration.ResponsibilityId;
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Remove(resCollaboration);
                    newResponsibility.IsRelationCollaboration = true;
                    newResponsibility.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Add(newResponsibility);

                }
                else if (drcCard.MainCardId != null && collaborationCard.MainCardId == null && (collaborationCard.Id == (int)drcCard.MainCardId))
                {

                    DrcCardResponsibility newResponsibility = new DrcCardResponsibility
                    {
                        Responsibility = resCollaboration.Responsibility,
                        ResponsibilityId = resCollaboration.ResponsibilityId
                    };
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Remove(resCollaboration);
                    newResponsibility.IsRelationCollaboration = true;
                    newResponsibility.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Add(newResponsibility);

                }
                else if (drcCard.MainCardId != null && (collaborationCard.Id == (int)drcCard.MainCardId))
                {

                    DrcCardResponsibility newResponsibility = new DrcCardResponsibility
                    {
                        Responsibility = resCollaboration.Responsibility,
                        ResponsibilityId = resCollaboration.ResponsibilityId
                    };
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Remove(resCollaboration);
                    newResponsibility.IsRelationCollaboration = true;
                    newResponsibility.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Add(newResponsibility);


                }
                else if (collaborationCard.MainCardId != null && collaborationCard.MainCardId == drcCard.Id)
                {

                    DrcCardResponsibility newResponsibility = new DrcCardResponsibility
                    {
                        Responsibility = resCollaboration.Responsibility,
                        ResponsibilityId = resCollaboration.ResponsibilityId
                    };
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Remove(resCollaboration);
                    newResponsibility.IsRelationCollaboration = true;
                    newResponsibility.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardResponsibilityRepository.Add(newResponsibility);

                }


            }


        }
        private void reArrangeFieldCollaborationsAfterMove(ICollection<DrcCard> targetVersionCards, DrcCardField fieldCollaboration)
        {
            var collaborationCard = _documentTransferUnitOfWork.DrcCardRepository.GetById(fieldCollaboration.DrcCardId);

            foreach (var drcCard in targetVersionCards.ToList())
            {
                if (drcCard.MainCardId != null && collaborationCard.MainCardId != null && (collaborationCard.MainCardId == (int)drcCard.MainCardId))
                {
                    DrcCardField field = new DrcCardField();
                    field.Field = fieldCollaboration.Field;
                    field.FieldId = fieldCollaboration.FieldId;
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Remove(fieldCollaboration);
                    field.IsRelationCollaboration = true;
                    field.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Add(field);



                }
                else if (drcCard.MainCardId != null && collaborationCard.MainCardId == null && (collaborationCard.Id == (int)drcCard.MainCardId))
                {
                    DrcCardField field = new DrcCardField();
                    field.Field = fieldCollaboration.Field;
                    field.FieldId = fieldCollaboration.FieldId;
                    field.IsRelationCollaboration = true;
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Remove(fieldCollaboration);
                    field.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Add(field);


                }
                else if (drcCard.MainCardId != null && (collaborationCard.Id == (int)drcCard.MainCardId))
                {
                    DrcCardField field = new DrcCardField();

                    field.Field = fieldCollaboration.Field;
                    field.IsRelationCollaboration = true;
                    field.DrcCard = drcCard;
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Remove(fieldCollaboration);
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Add(field);



                }
                else if (collaborationCard.MainCardId != null && collaborationCard.MainCardId == drcCard.Id)
                {

                    DrcCardField field = new DrcCardField
                    {
                        Field = fieldCollaboration.Field,
                        IsRelationCollaboration = true,
                        DrcCard = drcCard
                    };
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Remove(fieldCollaboration);
                    _documentTransferUnitOfWork.DrcCardFieldRepository.Add(field);

                }

            }

        }




        private List<DrcCard> getCollaborationCardsofACard(int cardId)
        {
            List<DrcCard> collaborationCardsofACard = new List<DrcCard>();

            var drcCard = _documentTransferUnitOfWork.DrcCardRepository.getDrcCardWithAllEntities(cardId);
            foreach (var res in drcCard.DrcCardResponsibilities)
            {
                foreach (var resCol in res.Responsibility.DrcCardResponsibilities)
                {
                    if (resCol.IsRelationCollaboration)
                    {
                        collaborationCardsofACard.Add(_documentTransferUnitOfWork.DrcCardRepository.GetById(resCol.DrcCardId));
                    }
                }
            }
            foreach (var field in drcCard.DrcCardFields)
            {
                foreach (var fieldCol in field.Field.DrcCardFields)
                {
                    if (fieldCol.IsRelationCollaboration)
                    {
                        collaborationCardsofACard.Add(_documentTransferUnitOfWork.DrcCardRepository.GetById(fieldCol.DrcCardId));
                    }
                }

            }

            return collaborationCardsofACard;
        }
        //eğer relation kurulan documanın taşındığı yer orjinal yeri ise shadow oluşturulmaz merge edilir 
        private void createShadows(List<DrcCard> collaborationCards, int targetVersionId)
        {
            var targetVersion = _documentTransferUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(targetVersionId);


            foreach (var card in collaborationCards)
            {
                if (card.MainCardId != null)        //shadow
                {
                    bool needToCreateShadow = true;

                    foreach (var drcDocument in targetVersion.DRCards)
                    {
                        if (drcDocument.Id == (int)card.MainCardId)
                        {
                            needToCreateShadow = false;
                        }
                        else if (drcDocument.MainCardId != null && (int)card.MainCardId == (int)drcDocument.MainCardId)
                        {
                            needToCreateShadow = false;
                        }
                    }

                    if (needToCreateShadow)
                    {
                        var sourceName = _documentTransferUnitOfWork.DrcCardRepository.getDrcCardName((int)card.MainCardId);
                        DrcCard newShadowCard = new DrcCard();
                        newShadowCard.Id = 0;
                        newShadowCard.SubdomainVersionId = targetVersionId;
                        newShadowCard.DrcCardName = sourceName + " Shadow";
                        newShadowCard.MainCardId = card.MainCardId;
                        _documentTransferUnitOfWork.DrcCardRepository.Add(newShadowCard);

                    }


                }
                else //normal document
                {
                    bool needToCreateShadow = true;
                    foreach (var drcDocument in targetVersion.DRCards)
                    {
                        if (drcDocument.MainCardId == card.Id)
                        {
                            needToCreateShadow = false;
                        }
                    }

                    if (needToCreateShadow)
                    {
                        DrcCard newShadowCard = new DrcCard();
                        newShadowCard.Id = 0;
                        newShadowCard.DrcCardName = card.DrcCardName + " Shadow";
                        newShadowCard.SubdomainVersionId = targetVersionId;
                        newShadowCard.MainCardId = card.Id;
                        _documentTransferUnitOfWork.DrcCardRepository.Add(newShadowCard);

                    }


                }
            }

        }
        public String checkIfDocumentConnectedToCurrentVersion(int drcCardId)
        {

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



            _documentTransferUnitOfWork.Complete();
            return connectedDocuments;
        }
    }

}
