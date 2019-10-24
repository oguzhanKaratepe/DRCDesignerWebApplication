using System;
using System.Collections.Generic;
using System.Text;

namespace DRCDesigner.Entities.Concrete
{
   public class SubdomainVersionRole
    {
        public int SubdomainVersionId{ get; set; }

        public virtual SubdomainVersion SubdomainVersion { get; set; }

        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
 