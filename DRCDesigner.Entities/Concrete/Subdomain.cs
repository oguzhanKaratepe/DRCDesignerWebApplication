
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
            SubdomainVersions=new List<SubdomainVersion>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        [DisplayName("Subdomain Name")]
        [Required(ErrorMessage = "Subdomain Name is required!")]
        public string SubdomainName { get; set; }

        [JsonIgnore]
        public virtual ICollection<SubdomainVersion> SubdomainVersions {get; set;}

    }
}