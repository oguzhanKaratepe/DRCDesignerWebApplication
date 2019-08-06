using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.Models
{
    public class AuthorizationRole
    {
        public int AuthorizationId { get; set; }
        public virtual Authorization Authorization { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
