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
using System.Collections;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleUnitOfWork _roleUnitOfWork;

        public RolesController(IRoleUnitOfWork roleUnitOfWorkt)
        {
            _roleUnitOfWork = roleUnitOfWorkt;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> Get(DataSourceLoadOptions loadOptions)
        {

            IEnumerable<Role> roles = await _roleUnitOfWork.RoleRepository.GetAll();

            return DataSourceLoader.Load(roles, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var newRole = new Role();
            JsonConvert.PopulateObject(values, newRole);

            if (!TryValidateModel(newRole))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"

            _roleUnitOfWork.RoleRepository.Add(newRole);
            _roleUnitOfWork.Complete();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(key);

            JsonConvert.PopulateObject(values, role);

            if (!TryValidateModel(role))
                return BadRequest("I will add error to here");// örnek var bununla ilgili dev extreme "ModelState.GetFullErrorMessage()"
            _roleUnitOfWork.Complete();

            return Ok();
        }
        [HttpDelete]
        public void Delete(int key)
        {
            var role = _roleUnitOfWork.RoleRepository.GetById(key);
            _roleUnitOfWork.RoleRepository.Remove(role);
            _roleUnitOfWork.Complete();
        }


    }

}
