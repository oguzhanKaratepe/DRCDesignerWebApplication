using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DRCDesignerWebApplication.Controllers
{
    public class DataGridSubdomainsLookup:Controller
    {
      private readonly ISubdomainUnitOfWork _subdomainUnitOfWork;

        public DataGridSubdomainsLookup(ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork= subdomainUnitOfWork;
        }
        [HttpGet]
        public async Task<object> Get(DataSourceLoadOptions loadOptions)
        {

            IEnumerable<Subdomain> subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAll();
            
            return DataSourceLoader.Load(subdomains, loadOptions);
        }

    }
}