using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using DRCDesigner.Business.Abstract;
using DRCDesignerWebApplication.ViewModels;
using DRCDesigner.Entities.Concrete;


namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainVersionExportController : Controller
    {
        private readonly IExportService _exportService;
        public SubdomainVersionExportController(IExportService exportService)
        {
            _exportService = exportService;
        }


       [HttpGet]
        public IActionResult GenerateReportUrl(int subdomainId)
        {
            if (subdomainId > 0)
            {
                string result = _exportService.generateSubdomainVersionReportUrl(subdomainId);

                if (string.IsNullOrEmpty(result))
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        public IActionResult DownloadSubdomainVersion(int subdomainId)
        {
            if (subdomainId > 0)
            {
                _exportService.generateSubdomainVersionDocuments(subdomainId);

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
