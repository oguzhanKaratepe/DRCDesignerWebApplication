using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesignerWebApplication.DAL.UnitOfWork;
using DRCDesignerWebApplication.DAL.UnitOfWork.Abstract;
using DRCDesignerWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class ResponsibilitiesController : Controller
    {
        private readonly IDrcUnitOfWork _drcUnitOfWork;
        public ResponsibilitiesController(IDrcUnitOfWork drcUnitOfWork)
        {
            _drcUnitOfWork = drcUnitOfWork;

        }



        [HttpGet]
        public async Task<object> Get(DataSourceLoadOptions loadOptions)
        {
            List<Responsibility> responsibilities = new List<Responsibility>();

            return DataSourceLoader.Load(responsibilities, loadOptions);
        }

    }
}