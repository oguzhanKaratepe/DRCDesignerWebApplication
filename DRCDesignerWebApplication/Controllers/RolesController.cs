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

namespace DRCDesignerWebApplication.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRoleUnitOfWork _roleUnitOfWork;

        public RolesController(IRoleUnitOfWork roleUnitOfWorkt)
        {
            _roleUnitOfWork = roleUnitOfWorkt;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {

            var roles = await _roleUnitOfWork.RoleRepository.getRoles();
            return View(roles);
        }

        // GET: Roles/Create
        public async Task<IActionResult> CreateOrUpdate(int id = 0)
        {
            var subdomains = await _roleUnitOfWork.SubdomainRepository.GetAll();

            ViewData["SubdomainId"] = new SelectList(subdomains, "Id", "SubdomainName");
            if (id == 0)
                return View(new Role());
            else
                return View(_roleUnitOfWork.RoleRepository.GetById(id));

        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate([Bind("Id,RoleName,SubdomainId")] Role role)
        {
            if (ModelState.IsValid)
            {
                if (role.Id == 0)
                    _roleUnitOfWork.RoleRepository.Add(role);
                else
                    _roleUnitOfWork.RoleRepository.update(role);

                _roleUnitOfWork.Complete();

                return RedirectToAction(nameof(Index));
            }
            var subdomains = await _roleUnitOfWork.SubdomainRepository.GetAll();
            ViewData["SubdomainId"] = new SelectList(subdomains, "Id", "SubdomainName", role.SubdomainId);
            return View(role);
        }


        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var role = _roleUnitOfWork.RoleRepository.GetById(id);
            if (role == null)
            {
                return NotFound();
            }
            _roleUnitOfWork.RoleRepository.remove(role);
            _roleUnitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }


    }

}
