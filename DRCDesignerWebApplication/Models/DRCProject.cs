using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DRCDesignerWebApplication.Models
{
    public class DrcProject : BaseEntity
    {

        public DrcProject()
        {
            Subdomains = new List<DrcCard>();
        }
        public int Id { get; set; }
        public string ProjectName {get; set;}
        public virtual ICollection<DrcCard> Subdomains { get; set; }

    }
}
