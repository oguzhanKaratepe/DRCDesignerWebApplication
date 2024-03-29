﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        private readonly IMapper _mapper;
        private readonly IDrcCardService _drcCardService;
        private readonly IDrcCardMoveService _drcCardMoveService;
        public DrcCardsController(IDrcCardService drcCardService, IMapper mapper, IDrcCardMoveService drcCardMoveService)
        {

            _mapper = mapper;
            _drcCardService = drcCardService;
            _drcCardMoveService = drcCardMoveService;
        }

        [HttpGet]
        public async Task<object> Index()
        {
            int id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            if (id >= 0) {
            DrcCardContainerViewModel drcCardContainerViewModel = new DrcCardContainerViewModel();
            DrcCardViewModel drcCardViewModel;
            if (id >=0)
            {
                var tempCards = await _drcCardService.GetAllDrcCards(id);

                foreach (var card in tempCards)
                {
                    drcCardViewModel = _mapper.Map<DrcCardViewModel>(card);


                    foreach (var responsibilityBusinessModel in _drcCardService.getListOfDrcCardResponsibilities(card.Id))
                    {
                        drcCardViewModel.Responsibilities.Add(_mapper.Map<ResponsibilityViewModel>(responsibilityBusinessModel));
                    }


                    foreach (var fieldBusinessModel in _drcCardService.getListOfDrcCardFields(card.Id, card.MainCardId))
                    {
                        drcCardViewModel.Fields.Add(_mapper.Map<FieldViewModel>(fieldBusinessModel));
                    }


                    foreach (var authorizationBusinessModel in _drcCardService.getListOfDrcCardAuthorizations(card.Id))
                    {
                        drcCardViewModel.Authorizations.Add(_mapper.Map<AuthorizationViewModel>(authorizationBusinessModel));
                    }

                    drcCardViewModel.SourceDrcCardPath = _drcCardService.GetShadowCardSourcePath(card.MainCardId);
                    drcCardContainerViewModel.DrcCardViewModes.Add(drcCardViewModel);

                }


                drcCardContainerViewModel.DrcCardViewModel.SubdomainVersionId = id;
            }
            drcCardContainerViewModel.TotalSubdomainSize = _drcCardService.TotalSubdomainSize();
            drcCardContainerViewModel.IsSubdomainVersionLocked = _drcCardService.isSubdomainVersionLocked(id);


            return View(drcCardContainerViewModel);
            }
            else
            {
                ViewData["Message"] = "version Id passed as negatif number";
                ViewData["SubdomainVersionId"] = 0;

                return View("ErrorPage");
            }
        }
        [HttpGet]
        public async Task<object> Presentation(string searchText)
        {

            String querySubdomain = HttpContext.Request.Query["subdomain"];
            String queryVersion = HttpContext.Request.Query["version"];

            DrcCardContainerViewModel drcCardContainerViewModel = new DrcCardContainerViewModel();
            int id = _drcCardService.getVersionIdFromQueryString(querySubdomain, queryVersion);
            if (id > 0)
            {


                if (id != 0)
                {
                    var tempCards = await _drcCardService.GetAllDrcCards(id);

                    foreach (var card in tempCards)
                    {
                        DrcCardViewModel drcCardViewModel = _mapper.Map<DrcCardViewModel>(card);


                        foreach (var responsibilityBusinessModel in _drcCardService.getListOfDrcCardResponsibilities(card.Id))
                        {
                            drcCardViewModel.Responsibilities.Add(_mapper.Map<ResponsibilityViewModel>(responsibilityBusinessModel));
                        }


                        foreach (var fieldBusinessModel in _drcCardService.getListOfDrcCardFields(card.Id, card.MainCardId))
                        {
                            drcCardViewModel.Fields.Add(_mapper.Map<FieldViewModel>(fieldBusinessModel));
                        }


                        foreach (var authorizationBusinessModel in _drcCardService.getListOfDrcCardAuthorizations(card.Id))
                        {
                            drcCardViewModel.Authorizations.Add(_mapper.Map<AuthorizationViewModel>(authorizationBusinessModel));
                        }

                        drcCardViewModel.SourceDrcCardPath = _drcCardService.GetShadowCardSourcePath(card.MainCardId);
                        drcCardContainerViewModel.DrcCardViewModes.Add(drcCardViewModel);

                    }

                    drcCardContainerViewModel.PresentationHeader = _drcCardService.GetPresentationHeader(id);
                    drcCardContainerViewModel.DrcCardViewModel.SubdomainVersionId = id;
                }
                return View(drcCardContainerViewModel);
            }

            drcCardContainerViewModel.ErrorMessage = "The address you entered is not available";

            return View(drcCardContainerViewModel);

        }


        [HttpGet]
        public async Task<object> GetSubdomainsWithVersions(int Id, DataSourceLoadOptions loadOptions)
        {
            var SubdomainsWithVersions = await _drcCardService.GetAllSubdomainMenuItems(Id);

            return DataSourceLoader.Load(SubdomainsWithVersions, loadOptions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostShadow(DrcCardViewModel drcCardViewModel)
        {
            var shadowCard = _mapper.Map<DrcCard>(drcCardViewModel);
            shadowCard.MainCardId = shadowCard.Id;
            shadowCard.Id = 0;
            if (!TryValidateModel(shadowCard))
            {
                ViewData["Message"] = "model is not valid";
                ViewData["SubdomainVersionId"] = shadowCard.SubdomainVersionId;

                return View("ErrorPage");
            }
             

            var result = _drcCardService.AddShadowCard(shadowCard);

            if (String.IsNullOrEmpty(result))
            {
                return Redirect("/DrcCards/index?id=" + shadowCard.SubdomainVersionId);
            }
            else
            {
                ViewData["Message"] = result;
                ViewData["SubdomainVersionId"] = shadowCard.SubdomainVersionId;

                return View("ErrorPage");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(DrcCardViewModel drcCardViewModel)
        {

            var newDrcCard = _mapper.Map<DrcCardBusinessModel>(drcCardViewModel);
            _drcCardService.Add(newDrcCard);


            return Redirect("/DrcCards/index?id=" + drcCardViewModel.SubdomainVersionId);
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
            {
                _drcCardService.Update(key, values);
            }
            else
            {
                return BadRequest("I will add error to here");
            }

            return Ok();

        }
        [HttpGet]
        public async Task<object> GetCardCollaborationOptions(int cardId, DataSourceLoadOptions loadOptions)
        {
            var cards = await _drcCardService.GetCardCollaborationOptions(cardId);
            return DataSourceLoader.Load(cards, loadOptions);
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromBody]DrcCardViewModel model)
        {

            string IsdocumentUsedAsCollaboration = _drcCardMoveService.checkIfDocumentConnectedToCurrentVersion(model.Id);

            if (String.IsNullOrEmpty(IsdocumentUsedAsCollaboration))
            {
                string result = await _drcCardService.Delete(model.Id);

                if (string.IsNullOrEmpty(result))
                {

                    return Ok();
                }
                else
                {
                    return BadRequest("Please delete the shadows of this document. You have fallowing shadows of this document; " + result);
                }
            }
            else
            {
                return BadRequest("To be able to delete this document you must first delete relations with fallowing documents: " + IsdocumentUsedAsCollaboration);
            }



        }

        [HttpGet]
        public async Task<object> GetShadowCardsSelectBox(int subdomainVersionId, DataSourceLoadOptions loadOptions)
        {
            var shadowCardSelectBoxOptions = await _drcCardService.GetShadowSelectBoxOptions(subdomainVersionId);
            var dropBoxSelectOptions = _mapper.Map<IList<ShadowCardSelectBoxBusinessModel>, IList<ShadowCardSelectBoxViewModel>>(shadowCardSelectBoxOptions);
            return DataSourceLoader.Load(dropBoxSelectOptions, loadOptions);
        }

        [HttpGet]
        public async Task<object> GetCard(int Id, DataSourceLoadOptions loadOptions)

        {
            var cardViewModel = _mapper.Map<DrcCardViewModel>(_drcCardService.GetCard(Id));

            List<DrcCardViewModel> viewModel = new List<DrcCardViewModel>();
            viewModel.Add(cardViewModel);
            return DataSourceLoader.Load(viewModel, loadOptions);
        }

        [HttpPost]
        public async Task<IActionResult> MoveCardToDestinationSubdomain([FromBody]DrcCardViewModel drcCardViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var checkReferences = await _drcCardMoveService.CheckMoveOperationReferenceNeeds(drcCardViewModel.Id, drcCardViewModel.SubdomainVersionId);

                    if (string.IsNullOrWhiteSpace(checkReferences))
                    {
                        var result = await _drcCardMoveService.MoveCardToDestinationSubdomainAsync(drcCardViewModel.Id, drcCardViewModel.SubdomainVersionId, drcCardViewModel.DrcCardName);

                        if (result.MoveResultType == MoveResultType.Success)
                        {
                            return Ok(result);
                        }
                        else
                        {
                            return BadRequest(result.MoveResultDefinition);
                        }
                    }
                    else
                    {
                        return BadRequest(checkReferences);
                    }

                }
                else
                {
                    return BadRequest("I will add error to here");
                }
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                ViewData["SubdomainVersionId"] = 0;
                return View("ErrorPage");
            }
        }

        public static List<object> GetDeleteBehaviorOptions()
        {
            var items = new List<object>();
            foreach (var item in Enum.GetValues(typeof(EDeleteBehaviorOptions)))
            {
                var fieldInfo = item.GetType().GetField(item.ToString());
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];
                items.Add(new { Id = item, Name = descriptionAttributes[0].Name });
            }
            return items;
        }

    }
}