using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Business.BusinessModels;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace DRCDesignerWebApplication.Controllers
{
    public class DrcCardsController : Controller
    {
        private readonly IDrcUnitOfWork _drcUnitOfWork;
        private readonly IMapper _mapper;
        private readonly IDrcCardService _drcCardService;

        public DrcCardsController(IDrcCardService drcCardService, IDrcUnitOfWork drcUnitOfWork, IMapper mapper)
        {
            _drcUnitOfWork = drcUnitOfWork;
            _mapper = mapper;
            _drcCardService = drcCardService;
        }

        private IList<ResponsibilityViewModel> getListOfResponsibilities(int Id)
        {
            var responsibilitiesCollection =
                _drcUnitOfWork.DrcCardResponsibilityRepository.GetDrcCardResponsibilitiesByDrcCardId(Id);

            IList<ResponsibilityViewModel> responsibilityViewModels = new List<ResponsibilityViewModel>();
            ResponsibilityViewModel responsibilityViewModel;
            foreach (var responsibilityCollection in responsibilitiesCollection)
            {
                var responsibility =
                    _drcUnitOfWork.ResponsibilityRepository.GetById(responsibilityCollection.ResponsibilityId);
                responsibilityViewModel = _mapper.Map<ResponsibilityViewModel>(responsibility);
                responsibilityViewModels.Add(responsibilityViewModel);
            }

            foreach (var responsibilityModel in responsibilityViewModels)
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

            return responsibilityViewModels;
        }

        private IList<FieldViewModel> getListOfFields(int Id)
        {
            var drcCardFieldCollections = _drcUnitOfWork.DrcCardFieldRepository.GetDrcCardFieldsByDrcCardId(Id);
            IList<FieldViewModel> fieldViewModels = new List<FieldViewModel>();

            foreach (var drcCardFieldCollection in drcCardFieldCollections)
            {
                var field = _drcUnitOfWork.FieldRepository.GetById(drcCardFieldCollection.FieldId);
                var fieldViewModel = _mapper.Map<FieldViewModel>(field);
                fieldViewModels.Add(fieldViewModel);
            }

            foreach (var fieldViewModel in fieldViewModels)
            {
                var fieldCollaboration =
                    _drcUnitOfWork.DrcCardFieldRepository.GetFieldCollaborationByFieldId(fieldViewModel.Id);
                if (fieldCollaboration != null)
                {
                    fieldViewModel.CollaborationCard = _drcUnitOfWork.DrcCardRepository.GetById(fieldCollaboration.DrcCardId);
                }

            }

            return fieldViewModels;
        }
        [HttpGet]
        public async Task<object> Index(int id)
        {
            DrcCardContainerViewModel drcCardContainerViewModel = new DrcCardContainerViewModel();
            DrcCardViewModel drcCardViewModel;
            if (id != 0)
            {
                var tempCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomain(id);

                foreach (var card in tempCards)
                {
                    var FullCard = _drcUnitOfWork.DrcCardRepository.getDrcCardWithAllEntities(card.Id);
                    drcCardViewModel = _mapper.Map<DrcCardViewModel>(FullCard);
                    drcCardViewModel.Responsibilities = getListOfResponsibilities(card.Id);
                    drcCardViewModel.Fields = getListOfFields(card.Id);
                    drcCardViewModel.SourceDrcCardPath = _drcCardService.GetShadowCardSourcePath(FullCard.MainCardId);
                    //  drcCardViewModel.CollaborationCards = getListOfCollaborations(FullCard);
                    foreach (var authorization in drcCardViewModel.Authorizations)
                    {
                        var authroles = _drcUnitOfWork.AuthorizationRoleRepository.GetAuthorizationRolesByAuthorizationId(
                                authorization.Id);
                        foreach (var authrole in authroles)
                        {
                            var role = _drcUnitOfWork.RoleRepository.GetById(authrole.RoleId);
                            authorization.Roles.Add(role);
                        }
                    }



                    drcCardContainerViewModel.DrcCardViewModes.Add(drcCardViewModel);

                }

            }
            //I add TotalSubdomainSize field to drcCardViewModel because I need subdomain size to decide show Add Card Button.
            //Because if subdomain size is 0 then there is no meaning to show add card button; 
            drcCardContainerViewModel.TotalSubdomainSize = _drcUnitOfWork.SubdomainRepository.subdomainSize();
            drcCardContainerViewModel.DrcCardViewModel.SubdomainId = id;
            _drcUnitOfWork.Complete();
            return View(drcCardContainerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostShadow(DrcCardViewModel drcCardViewModel)
        {
            var shadowCard = _mapper.Map<DrcCard>(drcCardViewModel);
            shadowCard.MainCardId = shadowCard.Id;
            shadowCard.Id = 0;
            if (!TryValidateModel(shadowCard))
                return BadRequest("I will add error to here");
            _drcCardService.Add(shadowCard);

            return Redirect("/DrcCards/index?id=" + shadowCard.SubdomainId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(DrcCardViewModel drcCardViewModel)
        {
            var newDrcCard = _mapper.Map<DrcCard>(drcCardViewModel);
            newDrcCard.DrcCardName = "New Document";

            //if (!TryValidateModel(drcCardViewModel))
            //    return BadRequest("I will add error to here");

            _drcCardService.Add(newDrcCard);

            return Redirect("/DrcCards/index?id=" + newDrcCard.SubdomainId);
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var card = _drcUnitOfWork.DrcCardRepository.GetById(key);

            JsonConvert.PopulateObject(values, card);

            _drcUnitOfWork.DrcCardRepository.Update(card);
            _drcUnitOfWork.Complete();

            return Ok();
        }

        //[HttpPut]
        //[ValidateAntiForgeryToken]
        //public IActionResult Put(DrcCardViewModel drcCardViewModel)
        //{
        //    if (!TryValidateModel(drcCardViewModel))
        //        return BadRequest("I will add error to here");

        //    var updatedCard = _mapper.Map<DrcCard>(drcCardViewModel);

        //    _drcCardService.Update(updatedCard);

        //    return Redirect("/DrcCards/index?id=" + updatedCard.SubdomainId);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DrcCardContainerViewModel modelContainer)
        {
            var cardToDelete = _mapper.Map<DrcCard>(modelContainer.DrcCardViewModel);
            _drcCardService.Delete(cardToDelete);
           
            return Redirect("/DrcCards/index?id=" + cardToDelete.SubdomainId);
        }

        [HttpGet]
        public async Task<object> GetShadowCardsSelectBox(int SubdomainId, DataSourceLoadOptions loadOptions)
        {
            var shadowCardSelectBoxOptions = await _drcCardService.GetShadowSelectBoxOptions(SubdomainId);
         var dropBoxSelectOptions = _mapper.Map<IList<ShadowCardSelectBoxBusinessModel>, IList<ShadowCardSelectBoxViewModel>>(shadowCardSelectBoxOptions);
         return DataSourceLoader.Load(dropBoxSelectOptions, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetCard(int Id, DataSourceLoadOptions loadOptions)

        {
            var card = _drcUnitOfWork.DrcCardRepository.GetById(Id);

            var model = _mapper.Map<DrcCardViewModel>(card);

            List<DrcCardViewModel> viewModels= new List<DrcCardViewModel>();
            viewModels.Add(model);
            return DataSourceLoader.Load(viewModels, loadOptions);
        }

        [HttpPost]
        public ActionResult MoveCardToDestinationSubdomain(int destinationId, int cardId)
        {
            var result = _drcCardService.MoveCardToDestinationSubdomain(destinationId, cardId);
            if (result != true)
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }
    }
}