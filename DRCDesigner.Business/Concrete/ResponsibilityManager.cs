using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.Concrete
{
    public class ResponsibilityManager:IResponsibilityService
    {
        private IMapper _mapper;
        private IDrcUnitOfWork _drcUnitOfWork;
        public ResponsibilityManager(IDrcUnitOfWork drcUnitOfWork, IMapper mapper)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<ResponsibilityBusinessModel>> GetCardResponsibilities(int cardId)
        {
            var cardResponsibilitiesCollection =
                _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(cardId);

            var ResponsibilityModels = new List<ResponsibilityBusinessModel>();

            ResponsibilityBusinessModel responsibilityModel;

            foreach (var cardResponsibilityCollection in cardResponsibilitiesCollection)
            {
                responsibilityModel = new ResponsibilityBusinessModel();
                responsibilityModel.DrcCardId = cardId;
                var tempResponsibility = _drcUnitOfWork.ResponsibilityRepository.GetById(cardResponsibilityCollection.ResponsibilityId);
                responsibilityModel.Id = tempResponsibility.Id;
                responsibilityModel.ResponsibilityDefinition = tempResponsibility.ResponsibilityDefinition;
                responsibilityModel.Title = tempResponsibility.Title;
                responsibilityModel.PriorityOrder = tempResponsibility.PriorityOrder;
                responsibilityModel.IsMandatory = tempResponsibility.IsMandatory;
              
                var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(tempResponsibility.Id);
                if (responsibilityCollaborations.Count > 0)
                {
                    int i = 0;
                    int[] tempIds = new int[responsibilityCollaborations.Count];
                    foreach (var collaborationCollection in responsibilityCollaborations)
                    {
                        tempIds[i] = collaborationCollection.DrcCardId;
                       
                        i++;
                    }

                    responsibilityModel.ShadowCardIds = tempIds;
                }
                ResponsibilityModels.Add(responsibilityModel);
            }

            return ResponsibilityModels;
        }

        public void Add(string values)
        {
            var newResponsibilityModel = new ResponsibilityBusinessModel();

            JsonConvert.PopulateObject(values, newResponsibilityModel);
            if (newResponsibilityModel.ResponsibilityDefinition != null)
            {

                var newResponsibility = new Responsibility();
                newResponsibility.ResponsibilityDefinition = newResponsibilityModel.ResponsibilityDefinition;
                newResponsibility.IsMandatory = newResponsibilityModel.IsMandatory;
                newResponsibility.Title = newResponsibilityModel.Title;
                newResponsibility.PriorityOrder = newResponsibilityModel.PriorityOrder;
                _drcUnitOfWork.ResponsibilityRepository.Add(newResponsibility);


                var drcCardResponsibility = new DrcCardResponsibility();
                drcCardResponsibility.Responsibility = newResponsibility;
                drcCardResponsibility.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(newResponsibilityModel.DrcCardId);
                _drcUnitOfWork.DrcCardResponsibilityRepository.Add(drcCardResponsibility);

                DrcCardResponsibility drcCardResponsibilityWithShadow;
                if (newResponsibilityModel.ShadowCardIds != null)
                {

                    foreach (var collaborationCardId in newResponsibilityModel.ShadowCardIds)
                    {
                        drcCardResponsibilityWithShadow = new DrcCardResponsibility();
                        drcCardResponsibilityWithShadow.Responsibility = newResponsibility;
                        drcCardResponsibilityWithShadow.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(collaborationCardId);
                        drcCardResponsibilityWithShadow.IsRelationCollaboration = true;
                        _drcUnitOfWork.DrcCardResponsibilityRepository.Add(drcCardResponsibilityWithShadow);
                        _drcUnitOfWork.Complete();
                    }
                }

                _drcUnitOfWork.Complete();

            }
            else
            {
                //do nothing
            }
        }

        public void Update(int id, string values)
        {

            Responsibility responsibility = _drcUnitOfWork.ResponsibilityRepository.GetById(id);
            ResponsibilityBusinessModel responsibilityBusinessModel = _mapper.Map<ResponsibilityBusinessModel>(responsibility);
            _drcUnitOfWork.ResponsibilityRepository.Remove(responsibility);
            var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(id);

            int[] shadowIds = new int[responsibilityCollaborations.Count];
            int i = 0;
            foreach (var responsibilityCollaboration in responsibilityCollaborations)
            {
                shadowIds[i] = responsibilityCollaboration.DrcCardId;
                _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(responsibilityCollaboration);
                i++;
            }
            responsibilityBusinessModel.ShadowCardIds = shadowIds;
            JsonConvert.PopulateObject(values, responsibilityBusinessModel);

            var newResponsibility = _mapper.Map<Responsibility>(responsibilityBusinessModel);
            if (responsibilityBusinessModel.ShadowCardIds != null)
            {
                DrcCardResponsibility resCollaboration;
                foreach (var drcresponsibilityCollaborationCard in responsibilityBusinessModel.ShadowCardIds)
                {
                    resCollaboration = new DrcCardResponsibility();
                    resCollaboration.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(drcresponsibilityCollaborationCard);
                    resCollaboration.Responsibility = newResponsibility;
                    resCollaboration.IsRelationCollaboration = true;
                    newResponsibility.DrcCardResponsibilities.Add(resCollaboration);
                }
            }
            _drcUnitOfWork.ResponsibilityRepository.Add(newResponsibility);
            _drcUnitOfWork.Complete();
         
        }

        public void Delete(int id)
        {
            var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByResponsibilityId(id);
            foreach (var responsibilityCollaboration in responsibilityCollaborations)
            {
                _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(responsibilityCollaboration);

            }

            _drcUnitOfWork.ResponsibilityRepository.Remove(id);
            _drcUnitOfWork.Complete();
        }

        public async Task<IList<DrcCard>> GetResponsibilityShadows(int versionId, int cardId)
        {
            var drcCards = _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomainVersion(versionId);
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
    }
}
