
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesignerWebApplication.ViewModels;

namespace DRCDesignerWebApplication.ViewComponents
{
    public class SubdomainTabViewComponent:ViewComponent
    {
        private readonly IDrcUnitOfWork _subdomainUnitOfWork;


        public SubdomainTabViewComponent(IDrcUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;

        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            var model = new SubdomainListViewModel
            {
                 Subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAll(),

                CurrentSubdomain = Convert.ToInt32(HttpContext.Request.Query["id"])
               
            };
            return View(model);
        }
    }
}
