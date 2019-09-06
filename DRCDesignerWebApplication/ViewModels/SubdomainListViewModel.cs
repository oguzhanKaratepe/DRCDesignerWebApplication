using DRCDesigner.Entities.Concrete;
using System.Collections.Generic;

namespace DRCDesignerWebApplication.ViewModels
{
    public class SubdomainListViewModel
    {
        public IEnumerable<Subdomain> Subdomains{ get; set; }
        public int CurrentSubdomain { get; set; }
    }
}