
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
            DRCards = new List<DrcCard>();
            SubdomainRoles = new List<Role>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "SubdomainName")]
        [DisplayName("Subdomain Name")]
        public string SubdomainName { get; set; }

        public virtual ICollection<DrcCard> DRCards { get; set; }
        public virtual ICollection<Role> SubdomainRoles { get; set; }
    }
}