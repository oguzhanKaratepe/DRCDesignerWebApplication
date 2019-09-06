using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Entities.Concrete;

namespace DRCDesignerWebApplication.ViewModels
{
    public class RoleListViewModel
    {
        public IEnumerable<Role> Roles { get; set; }
    }
}
