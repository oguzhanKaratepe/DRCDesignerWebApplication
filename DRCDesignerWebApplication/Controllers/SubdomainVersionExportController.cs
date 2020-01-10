using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using DRCDesigner.Business.Abstract;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Hosting;
namespace DRCDesignerWebApplication.Controllers
{
    public class SubdomainVersionExportController : Controller
    {
        private readonly IExportService _exportService;
        private IHostingEnvironment _env { get; }
        public SubdomainVersionExportController(IExportService exportService, IHostingEnvironment env)
        {
            _exportService = exportService;
            _env = env;
        }


        [HttpGet]
        public IActionResult GenerateReportUrl(int subdomainId)
        {
            if (subdomainId > 0)
            {
                try
                {
                    var result = _exportService.generateSubdomainVersionReportHtml(subdomainId);
                    byte[] bytes = Encoding.ASCII.GetBytes(result[1]);

                    string filename = result[0] + ".html";

                    return File(bytes, "text/html",
                        filename); // recommend specifying the download file name for zips

                }
                catch (Exception e)
                {
                    ViewData["Message"] =e.Message;
                    ViewData["SubdomainVersionId"] = subdomainId;
                    return View("ErrorPage");
                }

            }
            else
            {
                ViewData["Message"] = "Something went wrong while generating report";
                ViewData["SubdomainVersionId"] = subdomainId;
                return View("ErrorPage");
            }

        }

        public async Task<IActionResult> DownloadSubdomainVersion(int subdomainId)
        {


            if (subdomainId > 0)
            {
                try
                {
                    byte[] data = _exportService.generateSubdomainVersionDocuments(subdomainId);

                    return File(data, "application/x-zip-compressed",
                        "DocumentStore.zip"); // recommend specifying the download file name for zips
                }
                catch (Exception e)
                {
                    ViewData["Message"] = e.Message;
                    ViewData["SubdomainVersionId"] = subdomainId;
                    return View("ErrorPage");
                }
              
            }
            else
            {
                ViewData["Message"] = "Something went wrong while generation code";
                ViewData["SubdomainVersionId"] = subdomainId;
                return View("ErrorPage");
            }


        }

        [HttpGet]
        public async Task<object> GetDexmoVersions(DataSourceLoadOptions loadOptions)
        {

            var DexmoVersions = _exportService.getDexmoVersionOptions();
            return DataSourceLoader.Load(DexmoVersions, loadOptions);
        }


    }






}
