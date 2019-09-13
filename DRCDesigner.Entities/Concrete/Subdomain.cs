
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DRCDesigner.Core.Entities;

namespace DRCDesigner.Entities.Concrete
{
    public class Subdomain : IEntity
    {


        public Subdomain()
        {
            ReferencedSubdomains = new List<Subdomain>();
            SubdomainVertions=new List<SubdomainVersion>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        [DisplayName("Subdomain Name")]
        public string SubdomainName { get; set; }

        public int? ReferenceId;
        public virtual ICollection<Subdomain> ReferencedSubdomains { get; set; }
        public virtual ICollection<SubdomainVersion> SubdomainVertions { get; set; }

    }
}