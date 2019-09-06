using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;

namespace DRCDesigner.Business.Concrete
{
    public class DrcCardManager : IDrcCardService
    {
        private IDrcUnitOfWork _drcUnitOfWork;
        private IDocumentTransferUnitOfWork _documentTransferUnitOfWork;

        public DrcCardManager(IDrcUnitOfWork drcUnitOfWork, IDocumentTransferUnitOfWork documentTransferUnitOfWork)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _documentTransferUnitOfWork = documentTransferUnitOfWork;

        }

        public void Add(DrcCard drcCard)
        {
            string[] commanRoleBag = new[] { "C", "R", "U", "D" };
            for (int i = 0; i < commanRoleBag.Length; i++)
            {
                var authorization = new Authorization();
                authorization.DrcCard = drcCard;
                authorization.OperationName = commanRoleBag[i];
                _drcUnitOfWork.AuthorizationRepository.Add(authorization);
            }

            _drcUnitOfWork.DrcCardRepository.Add(drcCard);
            _drcUnitOfWork.Complete();
        }

        public void Update(DrcCard drcCard)
        {
            var oldCard = _drcUnitOfWork.DrcCardRepository.GetById(drcCard.Id);
            oldCard.DrcCardName = drcCard.DrcCardName;
            _drcUnitOfWork.DrcCardRepository.Update(oldCard);
            _drcUnitOfWork.Complete();
        }

        public async void Delete(DrcCard drcCard)
        {
            var cardResponsibilityCollections = _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(drcCard.Id);
            if (cardResponsibilityCollections != null)
            {
                foreach (var cardResponsibilityCollection in cardResponsibilityCollections)
                {
                    var drcCardResponsibilityCollaborations =
                        _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(cardResponsibilityCollection
                                .ResponsibilityId);
                    if (drcCardResponsibilityCollaborations != null)
                    {
                        foreach (var drcCardResponsibilityCollaboration in drcCardResponsibilityCollaborations)
                        {
                            _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(drcCardResponsibilityCollaboration);
                        }
                    }
                    _drcUnitOfWork.ResponsibilityRepository.Remove(cardResponsibilityCollection.ResponsibilityId);
                }
            }

            var cardFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(drcCard.Id);

            if (cardFieldCollections != null)
            {
                foreach (var cardFieldCollection in cardFieldCollections)
                {
                    var drcCardFieldCollaboration =
                        _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(
                            cardFieldCollection.FieldId);
                    if (drcCardFieldCollaboration != null)
                    {
                        _drcUnitOfWork.DrcCardFieldRepository.Remove(drcCardFieldCollaboration);
                    }
                    _drcUnitOfWork.FieldRepository.Remove(cardFieldCollection.FieldId);
                }
            }

            var cardAuthorizations = await _drcUnitOfWork.AuthorizationRepository.GetAuthorizationsByDrcCardId(drcCard.Id);

            if (cardAuthorizations != null)
            {
                foreach (var cardAuthorization in cardAuthorizations)
                {
                    var autRoleCollections = _drcUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(cardAuthorization
                            .Id);
                    if (autRoleCollections != null)
                    {
                        foreach (var autRoleCollection in autRoleCollections)
                        {
                            _drcUnitOfWork.AuthorizationRoleRepository.Remove(autRoleCollection);
                        }
                    }

                    _drcUnitOfWork.AuthorizationRepository.Remove(cardAuthorization);
                }

            }

            if (drcCard.MainCardId != null)
            {
                _drcUnitOfWork.DrcCardResponsibilityRepository.RemoveAllDrcCardResponsibilityCollaborationsByDrcCardId(drcCard.Id);
                _drcUnitOfWork.DrcCardFieldRepository.RemoveDrcCardFieldCollaborationsByDrcCardId(drcCard.Id);
            }
            _drcUnitOfWork.DrcCardRepository.Remove(drcCard);
            _drcUnitOfWork.Complete();
        }

        public async Task<IList<ShadowCardSelectBoxBusinessModel>> GetShadowSelectBoxOptions(int id)
        {
            ShadowCardSelectBoxBusinessModel selectBoxCard;
            IList<ShadowCardSelectBoxBusinessModel> selectBoxCards = new List<ShadowCardSelectBoxBusinessModel>();
            IEnumerable<DrcCard> cards = await _drcUnitOfWork.DrcCardRepository.GetAll();

            foreach (var drcCard in cards)
            {
                if (drcCard.MainCardId == null)
                {
                    selectBoxCard = new ShadowCardSelectBoxBusinessModel();
                    selectBoxCard.Id = drcCard.Id;
                    selectBoxCard.DrcCardName = drcCard.DrcCardName;
                    selectBoxCard.SubdomainId = drcCard.SubdomainId;
                    var subdomain = _drcUnitOfWork.SubdomainRepository.GetById(drcCard.SubdomainId);
                    selectBoxCard.SubdomainName = subdomain.SubdomainName;

                    if (drcCard.SubdomainId != id)
                    {
                        selectBoxCards.Add(selectBoxCard);
                    }

                }
                else
                {
                    //do nothing
                }

            }

            return selectBoxCards;
        }

        public bool MoveCardToDestinationSubdomain(int destinationId, int cardId)
        {
            if (destinationId != 0 && cardId != 0)
            {
                var card = _drcUnitOfWork.DrcCardRepository.GetById(cardId);
                var cardResponsibilities = _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(cardId);
                List<int> createdShadowCardIds = new List<int>();
                foreach (var responsibilitycollection in cardResponsibilities)
                {
                    var resCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(
                        responsibilitycollection.ResponsibilityId);
                    foreach (var resShadow in resCollaborations)
                    {
                        var shadowCard = _drcUnitOfWork.DrcCardRepository.GetById(resShadow.DrcCardId);
                        if (!createdShadowCardIds.Contains(shadowCard.Id))
                        {
                            createdShadowCardIds.Add(shadowCard.Id);
                            DrcCard newShadowCard = new DrcCard();
                            newShadowCard.MainCardId = shadowCard.MainCardId;
                            newShadowCard.DrcCardName = shadowCard.DrcCardName;
                            newShadowCard.SubdomainId = destinationId;
                            _drcUnitOfWork.DrcCardRepository.Add(newShadowCard);
                        }

                    }
                }


                card.SubdomainId = destinationId;
                _drcUnitOfWork.DrcCardRepository.Update(card);
                _drcUnitOfWork.Complete();
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetShadowCardSourcePath(int? shadowId)
        {
            try
            {
                var Id = (int) shadowId;
                var card = _drcUnitOfWork.DrcCardRepository.GetById(Id);
                var subdomain = _drcUnitOfWork.SubdomainRepository.GetById(card.SubdomainId);
                string pathName = "(" + subdomain.SubdomainName + "." + card.DrcCardName + ")";
                return pathName;
            }
            catch
            {

            }
            return null;
        }
    }
}