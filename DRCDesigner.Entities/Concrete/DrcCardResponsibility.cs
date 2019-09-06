using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesigner.Entities.Concrete
{
    public class DrcCardResponsibility
    {
        [Key, Column(Order = 1)]
        public int DrcCardId { get; set; }

        public virtual DrcCard DrcCard { get; set; }

        [Key, Column(Order = 2)]
        public int ResponsibilityId { get; set; }
        public virtual Responsibility Responsibility { get; set; }

        public bool IsRelationCollaboration { get; set; }
    }
}
