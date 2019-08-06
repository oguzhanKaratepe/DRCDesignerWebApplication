using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DRCDesignerWebApplication.DAL.Context;
using DRCDesignerWebApplication.Models;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainsController : Controller
    {

        private ISubdomainUnitOfWork _subdomainUnitOfWork;
        public SubdomainsController(ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> Get(DataSourceLoadOptions loadOptions)
        {

            IEnumerable<Subdomain> subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAll();

            return DataSourceLoader.Load(subdomains, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var newSubdomain = new Subdomain();
            JsonConvert.PopulateObject(values, newSubdomain);

            if (!TryValidateModel(newSubdomain))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

            _subdomainUnitOfWork.SubdomainRepository.Add(newSubdomain);
            _subdomainUnitOfWork.Complete();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var subdomain = _subdomainUnitOfWork.SubdomainRepository.GetById(key);

            JsonConvert.PopulateObject(values, subdomain);

            if (!TryValidateModel(subdomain))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"
            _subdomainUnitOfWork.SubdomainRepository.Update(subdomain);
            _subdomainUnitOfWork.Complete();
            
            return Ok();
        }
        [HttpDelete]
        public void Delete(int key)
        {
            var subdomain = _subdomainUnitOfWork.SubdomainRepository.GetById(key);
            
            _subdomainUnitOfWork.SubdomainRepository.Remove(subdomain);
            _subdomainUnitOfWork.Complete();
        }

       
    }
}
