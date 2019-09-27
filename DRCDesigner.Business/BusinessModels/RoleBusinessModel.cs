using System;
using System.Collections.Generic;
using System.Text;

namespace DRCDesigner.Business.BusinessModels
{
   public class RoleBusinessModel
    {
        public RoleBusinessModel()
        {
        
        }
        public int Id { get; set; }

        public string RoleName { get; set; }

        public int[] SubdomainVersionRoleIds { get; set; }
    }
}
