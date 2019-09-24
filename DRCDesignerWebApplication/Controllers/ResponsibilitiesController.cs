using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class ResponsibilitiesController : Controller
    {
        private IResponsibilityService _responsibilityService;
        private  IMapper _mapper;
        public ResponsibilitiesController(IResponsibilityService responsibilityService,IDrcUnitOfWork drcUnitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _responsibilityService = responsibilityService;

        }

        [HttpGet]
        public async Task<object> Get(int Id, DataSourceLoadOptions loadOptions)
        {
            var ResponsibilityBModels = await _responsibilityService.GetCardResponsibilities(Id);
            
            return DataSourceLoader.Load(ResponsibilityBModels, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            if (ModelState.IsValid)
            {
                _responsibilityService.Add(values);
            }
            else
            {
                return BadRequest("I will add error to here");
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
            {
                _responsibilityService.Update(key, values);
            }
            else
            {
                return BadRequest("I will add error to here");
            }

            return Ok();
        }

        [HttpDelete]
        public void Delete(int key)
        {
            _responsibilityService.Delete(key);
           
        }

        [HttpGet]
        public async Task<object> GetResponsibilityShadows(int Id, int cardId, DataSourceLoadOptions loadOptions)
        {
            var cards = await _responsibilityService.GetResponsibilityShadows(Id, cardId);

            return DataSourceLoader.Load(cards, loadOptions);
        }
    }


}