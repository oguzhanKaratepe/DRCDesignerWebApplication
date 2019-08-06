
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DRCDesignerWebApplication.Models
{
    public class Subdomain
    {


        public Subdomain()
        {
            DRCards = new List<DrcCard>();
            SubdomainRoles = new List<Role>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ID")]
        public int ID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "SubdomainName")]
        [DisplayName("Subdomain Name")]
        public string SubdomainName { get; set; }

        public virtual ICollection<DrcCard> DRCards { get; set; }
        public virtual ICollection<Role> SubdomainRoles { get; set; }
    }
}