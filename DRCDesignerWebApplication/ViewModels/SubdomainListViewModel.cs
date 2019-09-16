using DRCDesigner.Entities.Concrete;
using System.Collections.Generic;

namespace DRCDesignerWebApplication.ViewModels
{
    public class SubdomainListViewModel
    {
        public SubdomainListViewModel()
        {
            Subdomains=new List<Subdomain>();
        }
        public IEnumerable<Subdomain> Subdomains{ get; set; }
        public int CurrentSubdomain { get; set; }
    }
}