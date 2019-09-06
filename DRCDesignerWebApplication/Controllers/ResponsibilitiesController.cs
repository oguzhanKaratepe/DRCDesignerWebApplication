using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class ResponsibilitiesController : Controller
    {
        private readonly IDrcUnitOfWork _drcUnitOfWork;
        private readonly IMapper _mapper;
        public ResponsibilitiesController(IDrcUnitOfWork drcUnitOfWork, IMapper mapper)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<object> Get(int Id, DataSourceLoadOptions loadOptions)

        {

            var cardResponsibilitiesCollection =
                _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(Id);

            var ResponsibilityModels = new List<ResponsibilityViewModel>();

            ResponsibilityViewModel responsibilityModel;

            foreach (var cardResponsibilityCollection in cardResponsibilitiesCollection)
            {
                responsibilityModel = new ResponsibilityViewModel();
                responsibilityModel.DrcCardId = Id;
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

            return DataSourceLoader.Load(ResponsibilityModels, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {

            var newResponsibilityModel = new ResponsibilityViewModel();

            JsonConvert.PopulateObject(values, newResponsibilityModel);
            if (newResponsibilityModel.ResponsibilityDefinition != null)
            {

                if (!TryValidateModel(newResponsibilityModel))
                    return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

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

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            Responsibility responsibility = _drcUnitOfWork.ResponsibilityRepository.GetById(key);
            ResponsibilityViewModel responsibilityViewModel = _mapper.Map<ResponsibilityViewModel>(responsibility);
            _drcUnitOfWork.ResponsibilityRepository.Remove(responsibility);
            var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository.GetResponsibilityCollaborationsByResponsibilityId(key);
            
            int[] shadowIds = new int[responsibilityCollaborations.Count];
            int i = 0;
            foreach (var responsibilityCollaboration in responsibilityCollaborations)
            {
                shadowIds[i] = responsibilityCollaboration.DrcCardId;
                _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(responsibilityCollaboration);
                i++;
            }
            responsibilityViewModel.ShadowCardIds = shadowIds;
            JsonConvert.PopulateObject(values, responsibilityViewModel);
          
            var newResponsibility = _mapper.Map<Responsibility>(responsibilityViewModel);
            if (responsibilityViewModel.ShadowCardIds != null) { 
            DrcCardResponsibility resCollaboration;
            foreach (var drcresponsibilityCollaborationCard in responsibilityViewModel.ShadowCardIds)
            {
                resCollaboration=new DrcCardResponsibility();
                resCollaboration.DrcCard = _drcUnitOfWork.DrcCardRepository.GetById(drcresponsibilityCollaborationCard);
                resCollaboration.Responsibility = newResponsibility;
                resCollaboration.IsRelationCollaboration = true;
               newResponsibility.DrcCardResponsibilities.Add(resCollaboration);
            }
            }
            _drcUnitOfWork.ResponsibilityRepository.Add(newResponsibility);
            _drcUnitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public async void Delete(int key)
        {
            var responsibilityCollaborations = _drcUnitOfWork.DrcCardResponsibilityRepository
                .GetResponsibilityAllRelationsByResponsibilityId(key);
            foreach (var responsibilityCollaboration in responsibilityCollaborations)
            {
                _drcUnitOfWork.DrcCardResponsibilityRepository.Remove(responsibilityCollaboration);
                
            }

            _drcUnitOfWork.ResponsibilityRepository.Remove(key);
            _drcUnitOfWork.Complete();
        }

        [HttpGet]
        public async Task<object> GetResponsibilityShadows(int Id, int cardId, DataSourceLoadOptions loadOptions)
        {
            var drcCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomain(Id);
            IList<DrcCard> cards = new List<DrcCard>();
            foreach (var card in drcCards)
            {
                if (card.Id != cardId)
                {
                    cards.Add(card);
                }
            }

            return DataSourceLoader.Load(cards, loadOptions);
        }
    }


}