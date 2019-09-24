using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class SubdomainVersionViewModel
    {
      
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }
       
        public string SubdomainName { get; set; }
        public int SubdomainId { get; set; }
        public string SourceVersionName { get; set; }

        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }


        [RegularExpression(@"(\d+\.)?(\d+\.)?(\*|\d+)$", ErrorMessage = "Version number must be valid")]
        [DisplayName("Version number")]
        [Required]
        public string VersionNumber { get; set; }

        [Required]
        public bool EditLock { get; set; }

        [DisplayName("Source Version")]
        public int? SourceVersionId { get; set; }

        public int[] ReferencedVersionIds { get; set; }



    }
}
