using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DRCDesigner.Core.Entities;

namespace DRCDesigner.Entities.Concrete
{
    public class Role:IEntity
    {
        public Role()
        {
            AuthorizationRoles = new List<AuthorizationRole>();
            SubdomainVersionRoles=new List<SubdomainVersionRole>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "RoleName")]
        public string RoleName{ get; set; }

        public bool IsGlobal { get; set; }
        [JsonIgnore]
        public virtual IList<SubdomainVersionRole> SubdomainVersionRoles { get; set; }
        [JsonIgnore]
        public virtual IList<AuthorizationRole> AuthorizationRoles { get; set; }
    }
}