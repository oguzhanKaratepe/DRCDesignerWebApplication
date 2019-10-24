using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [JsonProperty(PropertyName = "RoleName")]
        [Required]
        public string RoleName { get; set; }

        [DisplayName("Role Version Areas")]
        [Required]
        public int[] SubdomainVersionRoleIds { get; set; }

        public string RoleVersionNumbers { get; set; }


    }
}
