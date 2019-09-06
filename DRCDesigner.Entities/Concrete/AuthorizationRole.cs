using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesigner.Entities.Concrete
{
    public class AuthorizationRole
    {
        [Key, Column(Order = 1)]
        public int AuthorizationId { get; set; }
        public virtual Authorization Authorization { get; set; }

        [Key, Column(Order = 2)]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
