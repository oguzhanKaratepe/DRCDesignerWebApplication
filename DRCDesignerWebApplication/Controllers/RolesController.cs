using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using DRCDesigner.Business.Abstract;
using DRCDesigner.Entities.Concrete;
using DRCDesignerWebApplication.ViewModels;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> Get(int Id,DataSourceLoadOptions loadOptions)
        {
            RoleListViewModel roleListViewModel = new RoleListViewModel
            {
                Roles = await _roleService.GetAll()
            };

            return DataSourceLoader.Load(roleListViewModel.Roles, loadOptions);
        }

        [HttpPost]
        public IActionResult Post(int Id, string values)
        {
            if (ModelState.IsValid)
                _roleService.Add(values);

            else
                return BadRequest("I will add error to here");

            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int key, string values)
        {
            if (ModelState.IsValid)
                _roleService.Update(values, key);

            else
                return BadRequest("I will add error to here");

            return Ok();
        }
        [HttpDelete]
        public ActionResult Delete(int key)
        {
            if (!_roleService.Remove(key))
            {
                return BadRequest("I will add error to here");
            }

            return RedirectToAction(nameof(Index));
        }


    }

}
