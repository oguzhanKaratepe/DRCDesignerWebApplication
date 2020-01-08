using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class SubdomainViewModel
    {
        public SubdomainViewModel()
        {
            SubdomainVersions = new List<SubdomainVersionViewModel>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        [DisplayName("Subdomain Name")]
        [Required(ErrorMessage = "Subdomain Name is required!")]
        public string SubdomainName { get; set; }

        public string SubdomainNamespace { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubdomainVersionViewModel> SubdomainVersions { get; set; }
    }
}
