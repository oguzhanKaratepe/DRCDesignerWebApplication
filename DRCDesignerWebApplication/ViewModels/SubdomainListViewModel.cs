using DRCDesigner.Entities.Concrete;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class SubdomainListViewModel
    {
        public SubdomainListViewModel()
        {
            Subdomains=new List<Subdomain>();
        }

        [JsonIgnore]
        public IEnumerable<Subdomain> Subdomains{ get; set; }

        public int CurrentSubdomain { get; set; }
    }
}