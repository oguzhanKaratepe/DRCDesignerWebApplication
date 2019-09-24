
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DRCDesigner.Business.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesignerWebApplication.ViewModels;

namespace DRCDesignerWebApplication.ViewComponents
{
    public class SubdomainTabViewComponent:ViewComponent
    {
        private readonly ISubdomainVersionService _subdomainVersionService;
        private readonly ISubdomainUnitOfWork _subdomainUnitOfWork;
        private IMapper _mapper;

        public SubdomainTabViewComponent(ISubdomainVersionService subdomainVersionService, ISubdomainUnitOfWork subdomainUnitOfWork, IMapper mapper)
        {
            _subdomainVersionService = subdomainVersionService;
            _subdomainUnitOfWork = subdomainUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ViewViewComponentResult> InvokeAsync()
        {
            //IList<SubdomainViewModel> subdomainViewModels =new List<SubdomainViewModel>();
            //var subdomains =await _subdomainUnitOfWork.SubdomainRepository.GetAll();
            //foreach (var subdomain in subdomains)
            //{
            //    var SubdomainViewModel = _mapper.Map<SubdomainViewModel>(subdomain);
            //    var SubdomainVersions = await _subdomainVersionService.GetAllSubdomainVersions(SubdomainViewModel.Id);
            //    foreach (var BModelVersion in SubdomainVersions)
            //    {
            //        var viewModel = _mapper.Map<SubdomainVersionViewModel>(BModelVersion);
            //        SubdomainViewModel.SubdomainVersions.Add(viewModel);
            //    }
            //    subdomainViewModels.Add(SubdomainViewModel);
            //}

            var model = new SubdomainListViewModel
            {
                 Subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAllWithVersions(),

                 CurrentSubdomain = Convert.ToInt32(HttpContext.Request.Query["id"])
               
            };
            return View(model);
        }
    }
}
