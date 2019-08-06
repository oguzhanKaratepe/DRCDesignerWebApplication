using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DRCDesignerWebApplication.Models
{
    public class Role
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "ID")]
        public int ID{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "RoleName")]
        public string RoleName{ get; set; }

        
        public int? SubdomainId{ get; set; }
        public virtual Subdomain Subdomain { get; set; }
    }
}