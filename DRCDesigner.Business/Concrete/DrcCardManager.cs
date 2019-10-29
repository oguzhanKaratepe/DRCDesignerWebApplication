using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class DrcCardManager : IDrcCardService
    {
        private IDrcUnitOfWork _drcUnitOfWork;
   
        private IMapper _mapper;
        public DrcCardManager(IMapper mapper, IDrcUnitOfWork drcUnitOfWork, IDocumentTransferUnitOfWork documentTransferUnitOfWork)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SubdomainMenuItemBusinessModel>> GetAllSubdomainMenuItems(int versionId)
        {
            string pathName = "Subdomains";

            var subdomainsWithVersions = await _drcUnitOfWork.SubdomainRepository.GetAllWithVersions();
            IList<SubdomainMenuItemBusinessModel> subdomainMenuItems = new List<SubdomainMenuItemBusinessModel>();

            var menuRootItem = new SubdomainMenuItemBusinessModel();

            IList<SubdomainMenuItemBusinessModel> subdomains = new List<SubdomainMenuItemBusinessModel>();
            foreach (var subdomain in subdomainsWithVersions)
            {
                SubdomainMenuItemBusinessModel root = new SubdomainMenuItemBusinessModel();
                root.text = subdomain.SubdomainName;
                root.type = 0;
                root.disabled = false;

                IList<SubdomainMenuItemBusinessModel> versions = new List<SubdomainMenuItemBusinessModel>();
                foreach (var subdomainVersion in subdomain.SubdomainVersions)
                {
                    SubdomainMenuItemBusinessModel b = new SubdomainMenuItemBusinessModel();
                    b.Id = subdomainVersion.Id;
                    b.text = subdomainVersion.VersionNumber;
                    b.type = 1;
                    b.EditLock = subdomainVersion.EditLock;
                    versions.Add(b);
                    if (subdomainVersion.Id == versionId)
                    {
                        pathName = subdomain.SubdomainName + ":" + subdomainVersion.VersionNumber;
                        b.disabled = true;
                    }

                }

                root.items = versions;
                if (versions.Count < 1)
                {
                    root.disabled = true;
                }

                subdomains.Add(root);
            }

            menuRootItem.items = subdomains;
            subdomainMenuItems.Add(menuRootItem);
            menuRootItem.text = pathName;
            menuRootItem.type = 0;
            return subdomainMenuItems;
        }
        public void Add(DrcCardBusinessModel businessModeldrcCard)
        {
            var drcCard = _mapper.Map<DrcCard>(businessModeldrcCard);
            drcCard.DrcCardName = "New Document";

            string[] commanRoleBag = new[] { "Create", "Read", "Update", "Delete" };
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

        public void Update(int id, string values)
        {

            var oldCard = _drcUnitOfWork.DrcCardRepository.GetById(id);
            JsonConvert.PopulateObject(values, oldCard);

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

            // functions defined above are to delete card own instance, but that card could have relations with other cards.

            var cardConnectionsByOtherCardsOverResponsibility = _drcUnitOfWork.DrcCardResponsibilityRepository
                .GetShadowCardAllResponsibilityCollaborationsByDrcCardId(drcCard.Id);

            foreach (var cardConnection in cardConnectionsByOtherCardsOverResponsibility)
            {
                _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(cardConnection);
            }

            var cardConnectionsByOtherCardsOverField =
                _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldCollaborationsByDrcCardId(drcCard.Id);
            foreach (var cardConnection in cardConnectionsByOtherCardsOverField)
            {
                _drcUnitOfWork.DrcCardFieldRepository.Remove(cardConnection);
            }

            if (drcCard.MainCardId != null)
            {
                _drcUnitOfWork.DrcCardResponsibilityRepository.RemoveAllDrcCardResponsibilityCollaborationsByDrcCardId(drcCard.Id);
                _drcUnitOfWork.DrcCardFieldRepository.RemoveDrcCardFieldCollaborationsByDrcCardId(drcCard.Id);
            }
            _drcUnitOfWork.DrcCardRepository.Remove(drcCard);
            _drcUnitOfWork.Complete();
        }

        public async Task<IList<ResponsibilityBusinessModel>> getListOfDrcCardResponsibilities(int cardId)
        {
            var responsibilitiesCollection =
                _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(cardId);

            IList<ResponsibilityBusinessModel> responsibilityBusinessModels = new List<ResponsibilityBusinessModel>();
            ResponsibilityBusinessModel responsibilityBusinessModel;
            foreach (var responsibilityCollection in responsibilitiesCollection)
            {
                var responsibility =
                    _drcUnitOfWork.ResponsibilityRepository.GetById(responsibilityCollection.ResponsibilityId);
                responsibilityBusinessModel = _mapper.Map<ResponsibilityBusinessModel>(responsibility);
                responsibilityBusinessModels.Add(responsibilityBusinessModel);
            }

            foreach (var responsibilityModel in responsibilityBusinessModels)
            {
                var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(responsibilityModel.Id);
                if (responsibilityCollaborations != null)
                {
                    foreach (var colllaboration in responsibilityCollaborations)
                    {
                        responsibilityModel.ResponsibilityCollaborationCards.Add(_drcUnitOfWork.DrcCardRepository.GetById(colllaboration.DrcCardId));
                    }
                }
                else
                {
                    //do nothing
                }
            }

            return responsibilityBusinessModels;
        }

        public async Task<IList<FieldBusinessModel>> getListOfDrcCardFields(int cardId, int? mainCardId)
        {
            var drcCardFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(cardId);
            IList<FieldBusinessModel> fieldBusinessModels = new List<FieldBusinessModel>();


            if (mainCardId != null)
            {
                int sourceCardId = (int)mainCardId;
                var sourceFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(sourceCardId);


                foreach (var fieldCollection in sourceFieldCollections)
                {
                    var sourceFieldViewModel = _mapper.Map<FieldBusinessModel>(_drcUnitOfWork.FieldRepository.GetById(fieldCollection.FieldId));
                    sourceFieldViewModel.IsShadowField = true;
                    fieldBusinessModels.Add(sourceFieldViewModel);
                }

            }

            foreach (var drcCardFieldCollection in drcCardFieldCollections)
            {
                var field = _drcUnitOfWork.FieldRepository.GetById(drcCardFieldCollection.FieldId);
                var fieldViewModel = _mapper.Map<FieldBusinessModel>(field);
                fieldBusinessModels.Add(fieldViewModel);
            }

            foreach (var fieldBusinessModel in fieldBusinessModels)
            {
                var fieldCollaboration =
                    _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(fieldBusinessModel.Id);
                if (fieldCollaboration != null)
                {
                    fieldBusinessModel.CollaborationCard = _drcUnitOfWork.DrcCardRepository.GetById(fieldCollaboration.DrcCardId);
                }

            }


            _drcUnitOfWork.Complete();

            return fieldBusinessModels;
        }

        public async Task<IList<AuthorizationBusinessModel>> getListOfDrcCardAuthorizations(int cardId)
        {
            IList<AuthorizationBusinessModel> authorizationBusinessModels = new List<AuthorizationBusinessModel>();
            foreach (var authorization in await _drcUnitOfWork.AuthorizationRepository.GetAuthorizationsByDrcCardId(cardId))
            {
                var AuthorizationBusinessModel = _mapper.Map<AuthorizationBusinessModel>(authorization);
                var authroles = _drcUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(authorization.Id);
                foreach (var authrole in authroles)
                {
                    var role = _drcUnitOfWork.RoleRepository.GetById(authrole.RoleId);
                    AuthorizationBusinessModel.Roles.Add(role);

                }
                authorizationBusinessModels.Add(AuthorizationBusinessModel);
            }

            return authorizationBusinessModels;
        }

        public async Task<IList<DrcCardBusinessModel>> GetAllDrcCards(int subdomainVersionId)
        {
            IList<DrcCardBusinessModel> drcCardBusinessModels = new List<DrcCardBusinessModel>();
            foreach (var drcCard in await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(subdomainVersionId))
            {
                var cardBusinessModel = _mapper.Map<DrcCardBusinessModel>(drcCard);
                drcCardBusinessModels.Add(cardBusinessModel);
            }
            return drcCardBusinessModels;
        }


        public async Task<IList<ShadowCardSelectBoxBusinessModel>> GetShadowSelectBoxOptions(int subdomainVersionId)
        {
            ShadowCardSelectBoxBusinessModel selectBoxCard;
            IList<ShadowCardSelectBoxBusinessModel> selectBoxCards = new List<ShadowCardSelectBoxBusinessModel>();

            var Version =
                await _drcUnitOfWork.SubdomainVersionRepository.GetVersionWithReferencesById(subdomainVersionId);

            foreach (var referencedVersion in Version.ReferencedSubdomainVersions)
            {

                var referencedSubdomainVersionWithCards = await _drcUnitOfWork.SubdomainVersionRepository.GetSubdomainVersionCardsWithId(referencedVersion.ReferencedVersionId);
                var referencedVersionSubdomain = _drcUnitOfWork.SubdomainRepository.GetById(referencedSubdomainVersionWithCards.SubdomainId);

                foreach (var drcCard in referencedSubdomainVersionWithCards.DRCards)
                {
                    if (drcCard.MainCardId == null)
                    {
                        selectBoxCard = new ShadowCardSelectBoxBusinessModel();
                        selectBoxCard.Id = drcCard.Id;
                        selectBoxCard.DrcCardName = drcCard.DrcCardName;
                        selectBoxCard.SubdomainVersionId = drcCard.SubdomainVersionId;
                        selectBoxCard.CardSourcePath = referencedVersionSubdomain.SubdomainName + ":" + referencedSubdomainVersionWithCards.VersionNumber;
                        if (drcCard.SubdomainVersionId != subdomainVersionId)
                        {
                            selectBoxCards.Add(selectBoxCard);
                        }

                    }
                    else
                    {
                        //do nothing
                    }

                }
            }
            return selectBoxCards;

        }

     

        public string GetShadowCardSourcePath(int? shadowId)
        {
            try
            {
                var Id = (int)shadowId;
                var card = _drcUnitOfWork.DrcCardRepository.GetById(Id);
                var subdomainVersion = _drcUnitOfWork.SubdomainVersionRepository.GetById(card.SubdomainVersionId);
                var subdomainName = _drcUnitOfWork.SubdomainRepository.GetSubdomainName(subdomainVersion.SubdomainId);

                string pathName = "(" + subdomainName + " > " + subdomainVersion.VersionNumber + " > " + card.DrcCardName + ")";
                return pathName;
            }
            catch
            {

            }
            return null;
        }

        public DrcCardBusinessModel GetCard(int id)
        {
            var businessCardModel = _mapper.Map<DrcCardBusinessModel>(_drcUnitOfWork.DrcCardRepository.GetById(id));

            return businessCardModel;
        }

        public int TotalSubdomainSize()
        {
            return _drcUnitOfWork.SubdomainRepository.subdomainSize();
        }

        public bool isSubdomainVersionLocked(int id)
        {
            var subdomainVersion = _drcUnitOfWork.SubdomainVersionRepository.GetById(id);

            if (subdomainVersion != null)
            {
                return subdomainVersion.EditLock;
            }

            return false;

        }


        public async Task<IList<DrcCard>> GetCardCollaborationOptions(int cardId)
        {
            var selectedCard = _drcUnitOfWork.DrcCardRepository.GetById(cardId);
            var drcCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(selectedCard.SubdomainVersionId);
            IList<DrcCard> cards = new List<DrcCard>();
            foreach (var card in drcCards)
            {
                if (card.Id != cardId)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        public async void AddShadowCard(DrcCard drcCard)
        {
            var id = (int) drcCard.MainCardId;
            var fields = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(id);

            _drcUnitOfWork.DrcCardRepository.Add(drcCard);

            _drcUnitOfWork.Complete();
        }
    }
}