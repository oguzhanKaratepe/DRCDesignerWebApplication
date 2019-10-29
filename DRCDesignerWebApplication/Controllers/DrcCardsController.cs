using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            DrcCardContainerViewModel drcCardContainerViewModel = new DrcCardContainerViewModel();
            DrcCardViewModel drcCardViewModel;
            if (id != 0)
            {
                var tempCards = await _drcCardService.GetAllDrcCards(id);

                foreach (var card in tempCards)
                {
                    drcCardViewModel = _mapper.Map<DrcCardViewModel>(card);


                    foreach (var responsibilityBusinessModel in await _drcCardService.getListOfDrcCardResponsibilities(card.Id))
                    {
                        drcCardViewModel.Responsibilities.Add(_mapper.Map<ResponsibilityViewModel>(responsibilityBusinessModel));
                    }


                    foreach (var fieldBusinessModel in await _drcCardService.getListOfDrcCardFields(card.Id, card.MainCardId))
                    {
                        drcCardViewModel.Fields.Add(_mapper.Map<FieldViewModel>(fieldBusinessModel));
                    }


                    foreach (var authorizationBusinessModel in await _drcCardService.getListOfDrcCardAuthorizations(card.Id))
                    {
                        drcCardViewModel.Authorizations.Add(_mapper.Map<AuthorizationViewModel>(authorizationBusinessModel));
                    }
                    
                    drcCardViewModel.SourceDrcCardPath = _drcCardService.GetShadowCardSourcePath(card.MainCardId);
                    drcCardContainerViewModel.DrcCardViewModes.Add(drcCardViewModel);

                }
             
                
                drcCardContainerViewModel.DrcCardViewModel.SubdomainVersionId = id;
            }
            drcCardContainerViewModel.SubdomainMenuItems = await _drcCardService.GetAllSubdomainMenuItems(id);
            drcCardContainerViewModel.TotalSubdomainSize = _drcCardService.TotalSubdomainSize();
            drcCardContainerViewModel.IsSubdomainVersionLocked = _drcCardService.isSubdomainVersionLocked(id);
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
                return BadRequest(ModelState.GetFullErrorMessage());

            _drcCardService.AddShadowCard(shadowCard);

            return Redirect("/DrcCards/index?id=" + shadowCard.SubdomainVersionId);
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
            var cards= await _drcCardService.GetCardCollaborationOptions(cardId);
            return DataSourceLoader.Load(cards, loadOptions);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DrcCardContainerViewModel modelContainer)
        {
            var cardToDelete = _mapper.Map<DrcCard>(modelContainer.DrcCardViewModel);
            _drcCardService.Delete(cardToDelete);
           
            return Redirect("/DrcCards/index?id="+ cardToDelete.SubdomainVersionId);
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
            
            List<DrcCardViewModel> viewModel= new List<DrcCardViewModel>();
            viewModel.Add(cardViewModel);
            return DataSourceLoader.Load(viewModel, loadOptions);
        }

        [HttpPost]
        public async Task<IActionResult> MoveCardToDestinationSubdomain([FromBody]DrcCardViewModel drcCardViewModel)
        {
            if (ModelState.IsValid)
            {
                var result =await _drcCardMoveService.MoveCardToDestinationSubdomainAsync(drcCardViewModel.Id, drcCardViewModel.SubdomainVersionId, drcCardViewModel.DrcCardName);

                if (result.MoveResultType!=MoveResultType.Success)
                {
                    return BadRequest(result.MoveResultDefinition);
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                return BadRequest("I will add error to here");
            }

            

          

           return BadRequest("your Move result  succeed");
 
          return Ok(drcCardViewModel);
          //  return Redirect("/DrcCards/index?id=" + currentSubdomainVersionId);
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