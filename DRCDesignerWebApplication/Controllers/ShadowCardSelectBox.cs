using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesignerWebApplication.Controllers.ViewModels;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DRCDesignerWebApplication.Controllers
{
    public class ShadowCardSelectBox : Controller
    {
        private readonly IDrcUnitOfWork _drcUnitOfWork;


        public ShadowCardSelectBox(IDrcUnitOfWork drcUnitOfWork)
        {
            _drcUnitOfWork = drcUnitOfWork;
        }



        [HttpGet]
        public async Task<object> Get(int SubdomainId, DataSourceLoadOptions loadOptions)
        {

            ShadowCardSelectBoxViewModel selectBoxCard;
            IList<ShadowCardSelectBoxViewModel> selectBoxCards=new List<ShadowCardSelectBoxViewModel>();
            IEnumerable<DrcCard> cards = await _drcUnitOfWork.DrcCardRepository.GetAll();
            IEnumerable<Subdomain> subdomains = await _drcUnitOfWork.SubdomainRepository.GetAll();
            Subdomain subdomain;

            foreach (var drcCard in cards)
            {
                selectBoxCard = new ShadowCardSelectBoxViewModel();
                selectBoxCard.Id = drcCard.Id;
                selectBoxCard.DrcCardName = drcCard.DrcCardName;
                subdomain= subdomains.Where(s => s.ID == drcCard.SubdomainId).Single();
                selectBoxCard.SubdomainName = subdomain.SubdomainName;
                if (subdomain.ID != SubdomainId)
                {
                    selectBoxCards.Add(selectBoxCard);
                }
                else
                {
                    //do nothing
                }
            }

            return DataSourceLoader.Load(selectBoxCards, loadOptions);
        }
    }
}