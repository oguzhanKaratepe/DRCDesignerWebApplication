using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesignerWebApplication.Controllers.ViewModels;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class DrcCardsController : Controller
    {
        private readonly IDrcUnitOfWork _drcUnitOfWork;
     


        public DrcCardsController(IDrcUnitOfWork drcUnitOfWork)
        {
            _drcUnitOfWork = drcUnitOfWork;
        }

       
        [HttpGet]
        public async Task<object> Index(int id)
        {
            DrcCardViewModel drcCardViewModel = new DrcCardViewModel();
            drcCardViewModel.DrcCard = new DrcCard();
            if (id != 0)
            {
                var tempCards = await _drcUnitOfWork.DrcCardRepository.getAllCardsBySubdomain(id);
                foreach (var card in tempCards)
                {
                    var FullCard = _drcUnitOfWork.DrcCardRepository.getDrcCardWithAllEntities(card.Id);
                    drcCardViewModel.DrsCards.Add(FullCard);
                }
            }
            //I add TotalSubdomainSize field to drcCardViewModel because I need subdomain size to decide show Add Card Button.
            //Because if subdomain size is 0 then there is no meaning to show add card button; 
            drcCardViewModel.TotalSubdomainSize = _drcUnitOfWork.SubdomainRepository.subdomainSize();
            drcCardViewModel.DrcCard.SubdomainId = id;
        
            _drcUnitOfWork.Complete();

            return View(drcCardViewModel);
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(DrcCardViewModel newModel)
        {
            var newCard = newModel.DrcCard;
            newCard.Responsibilities = newModel.Responsibilities;
            newCard.Authorizations = newModel.Authorizations;
            newCard.Fields = newModel.Fields;
            

            if (newCard.Id != 0)
            {
                _drcUnitOfWork.DrcCardRepository.Update(newCard);
            }
            else
            {
                _drcUnitOfWork.DrcCardRepository.Add(newCard);
            }

            //if (!TryValidateModel(newCard))
            //    return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"
            _drcUnitOfWork.Complete();
            

            return Redirect("/DrcCards/index?id=" + newCard.SubdomainId);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public IActionResult Put(int key, string values)
        {
            var card = _drcUnitOfWork.DrcCardRepository.GetById(key);

            JsonConvert.PopulateObject(values, card);

            if (!TryValidateModel(card))

                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"
            _drcUnitOfWork.DrcCardRepository.Update(card);
            _drcUnitOfWork.Complete();
            return Ok();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(DrcCardViewModel viewModel)
        {
            var cardToDelete = viewModel.DrcCard;
            _drcUnitOfWork.DrcCardRepository.Remove(cardToDelete);
            _drcUnitOfWork.Complete();

            return Redirect("/DrcCards/index?id=" + cardToDelete.SubdomainId);
        }


       }
}