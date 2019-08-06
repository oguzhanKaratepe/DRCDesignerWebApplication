using DRCDesignerWebApplication.Models;
using System.Collections.Generic;

namespace DRCDesignerWebApplication.Controllers.ViewModels
{
    public class SubdomainListViewModel
    {
        public IEnumerable<Subdomain> Subdomains{ get; set; }
        public int CurrentSubdomain { get; set; }
    }
}