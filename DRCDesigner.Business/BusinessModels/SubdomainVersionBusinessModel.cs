using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.BusinessModels
{
    public class SubdomainVersionBusinessModel
    {
        public SubdomainVersionBusinessModel()
        {
       
            ReferencedVersionIds= new int[0];
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        public string SubdomainName { get; set; }
        public int SubdomainId { get; set; }
        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }
        public int? SourceVersionId { get; set; }
        public string SourceVersionName { get; set; }

        [DisplayName("Version number")]
        public string VersionNumber { get; set; }

        public bool EditLock { get; set; }

        public int[] ReferencedVersionIds { get; set; }
        public string References { get; set; }

    }
}
