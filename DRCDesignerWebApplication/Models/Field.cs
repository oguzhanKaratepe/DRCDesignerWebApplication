using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.Models
{
    public class Field
    {
        public int Id { get; set; }
        public int drcCardId { get; set; }
        public string AttributeName { get; set; }
        public FieldType type { get; set; }
        public virtual DrcCard DrcCard { get; set; }
     
    }
}
