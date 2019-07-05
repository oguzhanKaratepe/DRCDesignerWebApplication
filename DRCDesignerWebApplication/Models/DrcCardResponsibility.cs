using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.Models
{
    public class DrcCardResponsibility
    {
        [Key, Column(Order = 1)]
        public int DrcCardID { get; set; }

        public virtual DrcCard DrcCard { get; set; }

        [Key, Column(Order = 2)]
        public int ResponsibilityID { get; set; }
        public virtual Responsibility Responsibility { get; set; }
    }
}
