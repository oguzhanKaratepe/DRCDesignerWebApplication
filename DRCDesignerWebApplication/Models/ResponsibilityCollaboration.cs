using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.Models
{
    public class ResponsibilityCollaboration
    {
        public int Id { get; set; }
        public int DrcCardId { get; set; }
        public int ResponsibilityId { get; set; }

        public DrcCard DrcCard { get; set; }
        public Responsibility Responsiblity { get; set; }
    }
}
