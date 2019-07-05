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

namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainsController : Controller
    {
        private readonly ISubdomainUnitOfWork _subdomainUnitOfWork;

        public SubdomainsController(ISubdomainUnitOfWork subdomainUnitOfWork)
        {
            _subdomainUnitOfWork = subdomainUnitOfWork;
        }


        // GET: Subdomains
        public async Task<IActionResult> Index()
        {
            var subdomains = await _subdomainUnitOfWork.SubdomainRepository.GetAll();
            return View(subdomains);
        }


        // GET: Subdomains/Create
        public async Task<IActionResult> CreateOrUpdate(int id = 0)
        {

            if (id == 0)
                return View(new Subdomain());
            else
                return View(_subdomainUnitOfWork.SubdomainRepository.GetById(id));
        }

        // POST: Subdomains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdate([Bind("Id,SubdomainName,")]Subdomain subdomain)
        {
            if (ModelState.IsValid)
            {
                if (subdomain.Id == 0)
                    _subdomainUnitOfWork.SubdomainRepository.Add(subdomain);
                else
                    _subdomainUnitOfWork.SubdomainRepository.update(subdomain);
                _subdomainUnitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }

            return View(subdomain);
        }

        // GET: Subdomains/Delete/5
        public async Task<IActionResult> Delete(int id)
        {

            var subdomain = _subdomainUnitOfWork.SubdomainRepository.GetById(id);
            if (subdomain == null)
            {
                return NotFound();
            }
            _subdomainUnitOfWork.SubdomainRepository.remove(subdomain);
            _subdomainUnitOfWork.Complete();

            return RedirectToAction(nameof(Index));
        }

    }
}
