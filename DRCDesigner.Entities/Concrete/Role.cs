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
            AuthorizationRoles=new List<AuthorizationRole>();
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id{ get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "RoleName")]
        public string RoleName{ get; set; }

        public int? SubdomainVersionId{ get; set; }
        [JsonIgnore]
        public virtual SubdomainVersion SubdomainVersion { get; set; }
        public virtual IList<AuthorizationRole> AuthorizationRoles { get; set; }
    }
}