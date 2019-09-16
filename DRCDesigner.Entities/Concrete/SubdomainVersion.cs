
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DRCDesigner.Core.Entities;

namespace DRCDesigner.Entities.Concrete
{
    public class SubdomainVersion : IEntity
    {

        public SubdomainVersion()
        {
            ReferencedSubdomainVersions = new List<SubdomainVersion>();
            DRCards = new List<DrcCard>();
            SubdomainVersionRoles = new List<Role>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        public int? SubdomainId { get; set; }
        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }

        [DisplayName("Version number")]
        public string VersionNumber { get; set; }

        public bool EditLock { get; set; }

        public int? ReferencedSubdomainId;

        public virtual SubdomainVersion ReferencedSubdomainVersion { get; set; }

        public virtual ICollection<DrcCard> DRCards { get; set; }
        public virtual ICollection<Role> SubdomainVersionRoles { get; set; }
        public virtual ICollection<SubdomainVersion> ReferencedSubdomainVersions { get; set; }
    }
}