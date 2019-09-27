using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class RoleViewModel
    {

        public RoleViewModel()
        {
            SubdomainVersionRoleIds = new int[0];
        }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "RoleName")]
        public string RoleName { get; set; }

        [DisplayName("Role Version Areas")]
        public int[] SubdomainVersionRoleIds { get; set; }


    }
}
