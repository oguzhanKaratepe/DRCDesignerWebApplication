using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesigner.Entities.Concrete
{
    public class DrcCardField
    {
        [Key, Column(Order = 1)]
        public int DrcCardId { get; set; }

        public virtual DrcCard DrcCard { get; set; }

        [Key, Column(Order = 2)]
        public int FieldId { get; set; }
        public virtual Field Field { get; set; }

        public bool IsRelationCollaboration { get; set; }
    }

}
