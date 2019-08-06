using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesignerWebApplication.Models;

namespace DRCDesignerWebApplication.Controllers.ViewModels
{
    public class ShadowCardSelectBoxViewModel
    {
        public int Id { get; set; }
        public string DrcCardName { get; set; }
        public string SubdomainName { get; set; }
    }
}
