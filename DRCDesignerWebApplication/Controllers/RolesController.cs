using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public RolesController(IRoleService roleService,IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<object> Get(int Id,DataSourceLoadOptions loadOptions)
        {
            IList<RoleViewModel> roleViewModels=new List<RoleViewModel>();
            foreach (var roleBusinessModel in await _roleService.GetAll())
            {
                RoleViewModel roleViewModel = _mapper.Map<RoleViewModel>(roleBusinessModel);
               roleViewModels.Add(roleViewModel);
            }
            return DataSourceLoader.Load(roleViewModels, loadOptions);
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
        public async Task<ActionResult> Delete(int key)
        {
            if (!await _roleService.Remove(key))
            {
                return BadRequest("I will add error to here");
            }

            return RedirectToAction(nameof(Index));
        }


    }

}
