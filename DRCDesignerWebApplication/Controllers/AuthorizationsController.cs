using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DRCDesigner.Business.Abstract;
using DRCDesigner.DataAccess.UnitOfWork.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace DRCDesignerWebApplication.Controllers
{
    public class AuthorizationsController : Controller
    {
      
        private IMapper _mapper;
        private IRoleUnitOfWork _roleUnitOfWork;
        private IAuthorizationService _authorizationService;
        public AuthorizationsController(IRoleUnitOfWork roleUnitOfWork, IMapper mapper,IAuthorizationService authorizationService)
        {
            _roleUnitOfWork = roleUnitOfWork;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        
        [HttpGet]
        public async Task<object> Get(int Id, DataSourceLoadOptions loadOptions)
        {
            var authorizationBModels = await _authorizationService.GetCardAuthorizations(Id);
            
            return DataSourceLoader.Load(authorizationBModels, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            if (ModelState.IsValid)
            {
                _authorizationService.Add(values);
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
                _authorizationService.Update(key, values);
            }
            else
            {
                return BadRequest("I will add error to here");
            }

            return Ok();
            
        }

        [HttpDelete]
        public  void Delete(int key)
        {
            _authorizationService.Delete(key);
           
        }

        [HttpGet]
        public async Task<object> GetAuthorizationRoles(int Id, DataSourceLoadOptions loadOptions)
        {
            var roles = await _authorizationService.GetAuthorizationRoles(Id);

            return DataSourceLoader.Load(roles, loadOptions);
        }
    }
}
