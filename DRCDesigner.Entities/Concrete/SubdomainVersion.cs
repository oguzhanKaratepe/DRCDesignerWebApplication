
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DRCDesigner.Core.Entities;

namespace DRCDesigner.Entities.Concrete
{
    public class SubdomainVersion : IEntity
    {

        public SubdomainVersion()
        {
            ReferencedSubdomainVersions = new List<SubdomainVersionReference>();
            DRCards = new List<DrcCard>();
            SubdomainVersionRoles = new List<SubdomainVersionRole>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        public int SubdomainId { get; set; }

        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }

        [DisplayName("Version number")]
        public string VersionNumber { get; set; }

        public bool EditLock { get; set; }
  
        public int? SourceVersionId { get; set; }

        public string DexmoVersion { get; set;}

        public SubdomainVersion SourceSubdomainVersion { get; set; }

        [JsonIgnore]
        public virtual IList<SubdomainVersionRole> SubdomainVersionRoles { get; set; }

        public virtual ICollection<DrcCard> DRCards { get; set; }
 
        public virtual ICollection<SubdomainVersionReference> ReferencedSubdomainVersions { get; set; }
    }
}