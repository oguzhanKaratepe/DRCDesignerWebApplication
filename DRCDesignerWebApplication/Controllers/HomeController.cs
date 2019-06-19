using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DRCDesignerWebApplication.Models;

namespace DRCDesignerWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (MyDbContext dbContext=new MyDbContext())
            {
                
            }
            
                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
