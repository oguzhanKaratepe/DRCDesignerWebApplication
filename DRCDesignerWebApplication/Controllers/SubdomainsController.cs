using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using DRCDesigner.Business.Abstract;
using DRCDesignerWebApplication.ViewModels;
using   DRCDesigner.Entities.Concrete;

namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainsController : Controller
    {

        private readonly ISubdomainService _subdomainService;
        private readonly IMapper _mapper;
        public SubdomainsController(ISubdomainService subdomainService,IMapper mapper)
        {
            _subdomainService = subdomainService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var newSubdomainViewModel=new SubdomainViewModel();
            return View(newSubdomainViewModel);
        }

        [HttpGet]
        public async Task<object> Get(DataSourceLoadOptions loadOptions)
        {
            //IList<SubdomainViewModel> subdomainViewModels = new List<SubdomainViewModel>();
            //var subdomains = await _subdomainService.GetAll();
            //foreach (var subdomain in subdomains)
            //{
            //    var SubdomainViewModel = _mapper.Map<SubdomainViewModel>(subdomain);
            //     subdomainViewModels.Add(SubdomainViewModel);
            //}

            SubdomainListViewModel subdomainListViewModel=new SubdomainListViewModel
            {
                Subdomains=await _subdomainService.GetAll()
             };

            return DataSourceLoader.Load(subdomainListViewModel.Subdomains, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            if (ModelState.IsValid)
            {
                _subdomainService.Add(values);
            }
            else
                return BadRequest(ModelState.GetFullErrorMessage());

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
                _subdomainService.Update(values,key);

            else
                return BadRequest("I will add error to here");

            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> Delete(int key)
        {
            if (! await _subdomainService.Remove(key))
            {
                return BadRequest("I will add error to here");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<object> GetDropDownButtonSubdomains(int Id, DataSourceLoadOptions loadOptions)
        {
            //var dropDownBoxSubdomains = await _subdomainService.GetMoveDropDownBoxSubdomains(Id);
            //return DataSourceLoader.Load(dropDownBoxSubdomains, loadOptions);
            throw new NotImplementedException();
        }


    }
}
