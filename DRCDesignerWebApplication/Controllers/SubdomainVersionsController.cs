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
using DRCDesigner.Entities.Concrete;

namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainVersionsController : Controller
    {

        private readonly ISubdomainVersionService _subdomainVersionService;
        private readonly IMapper _mapper;
        public SubdomainVersionsController(ISubdomainVersionService subdomainVersionService, IMapper mapper)
        {
            _subdomainVersionService = subdomainVersionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<object> Get(int id, DataSourceLoadOptions loadOptions)
        {
            IList<SubdomainVersionViewModel> viewModels = new List<SubdomainVersionViewModel>();
            var subdomainVersions = await _subdomainVersionService.GetAllSubdomainVersions(id);
            foreach (var BModelVersion in subdomainVersions)
            {
                var viewmodel = _mapper.Map<SubdomainVersionViewModel>(BModelVersion);
                viewModels.Add(viewmodel);
            }
            return DataSourceLoader.Load(viewModels, loadOptions);
        }
        [HttpGet]
        public async Task<object> GetSourceOptions(int id, int subdomainId, DataSourceLoadOptions loadOptions)
        {
            IList<SubdomainVersionViewModel> viewModels = new List<SubdomainVersionViewModel>();
            var subdomainVersions = await _subdomainVersionService.GetAllSubdomainVersionSourceOptions(subdomainId,id);
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
        [HttpGet]
        public async Task<object> GetVersionExportOptions(DataSourceLoadOptions loadOptions)
        {
            var exportOptions = await _subdomainVersionService.GetExportOptions();
            return DataSourceLoader.Load(exportOptions, loadOptions);
        }
        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            if (ModelState.IsValid)
            {
                var CheckSourceLocked = _subdomainVersionService.Add(values);

                if (!await CheckSourceLocked)
                {
                    return BadRequest("Your source version locked. Please unlock source before create new one!");
                }
            }
            else
                return BadRequest(ModelState.GetFullErrorMessage());

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            if (ModelState.IsValid)
            {
                var SourceChange= await _subdomainVersionService.LookForSourceChange(key,values);

                if (!SourceChange)
                {
                    return BadRequest("You are not allowed to change source version!!");
                }
                 bool update= await _subdomainVersionService.Update(values, key);
                 if (update)
                 {
                     return Ok();
                 }
            }
            else
                return BadRequest("I will add error to here");

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int key)
        {
            if (await _subdomainVersionService.VersionIsASource(key))
            {
                return BadRequest(
                    "You are not allowed to delete this version because it is using as a source by some other versions");
            }
            else
            {
                if (! _subdomainVersionService.Remove(key))
                {
                    return BadRequest("I will add error to here");
                }

                return Ok();
            }

          
        }
        [HttpGet]
        public async Task<object> GetSubdomainVersions(int subdomainId, DataSourceLoadOptions loadOptions)
        {

            IList<SubdomainVersionViewModel> viewModels = new List<SubdomainVersionViewModel>();
            var subdomainVersions = await _subdomainVersionService.GetAllSubdomainVersions(subdomainId);
            foreach (var BModelVersion in subdomainVersions)
            {
                var viewmodel = _mapper.Map<SubdomainVersionViewModel>(BModelVersion);
                viewModels.Add(viewmodel);
            }
            return DataSourceLoader.Load(viewModels, loadOptions);
        
        }

    }
}
