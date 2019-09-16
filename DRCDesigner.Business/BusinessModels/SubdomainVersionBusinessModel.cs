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
            ReferencedSubdomainVersions =new List<SubdomainVersion>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        public string SubdomainName { get; set; }
        public int SubdomainId { get; set; }
        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }

        [DisplayName("Version number")]
        public string VersionNumber { get; set; }

        public bool EditLock { get; set; }

        public int[] ReferencedVersionIds { get; set; }
        public virtual ICollection<SubdomainVersion> ReferencedSubdomainVersions { get; set; }
    }
}
