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
    public class SubdomainVersionsController : Controller
    {

        private readonly ISubdomainVersionService _subdomainVersionService;
        private readonly IMapper _mapper;
        public SubdomainVersionsController(ISubdomainVersionService subdomainVersionService,IMapper mapper)
        {
            _subdomainVersionService = subdomainVersionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<object> Get(int id,DataSourceLoadOptions loadOptions)
        {
            IList<SubdomainVersionViewModel> viewModels=new List<SubdomainVersionViewModel>();
            var subdomainVersions = await _subdomainVersionService.GetAllSubdomainVersions(id);
            foreach (var BModelVersion in subdomainVersions)
            {
                var viewmodel = _mapper.Map<SubdomainVersionViewModel>(BModelVersion);
                viewModels.Add(viewmodel);
            }
            return DataSourceLoader.Load(viewModels, loadOptions);
        }
        [HttpGet]
        public async Task<object> GetReferenceOptions(int id, DataSourceLoadOptions loadOptions)
        {
            var refenceOptions = await _subdomainVersionService.GetReferenceOptions(id);
            return DataSourceLoader.Load(refenceOptions, loadOptions);
        }
       
        [HttpPost]
        public IActionResult Post(string values)
        {
        
            if (ModelState.IsValid)
                _subdomainVersionService.Add(values);

            else
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
                _subdomainVersionService.Update(values, key);

            else
                return BadRequest("I will add error to here");

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int key)
        {
            if (!await _subdomainVersionService.Remove(key))
            {
                return BadRequest("I will add error to here");
            }

            return Ok();
        }
    }
}
