using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DRCDesigner.Entities.Concrete
{
   public class SubdomainVersionReference
    {
   
        public int SubdomainVersionId { get; set; }

        public virtual SubdomainVersion SubdomainVersion { get; set; }

        public int ReferencedVersionId { get; set; }
        public virtual SubdomainVersion ReferencedSubdomainVersion { get; set; }

    }
}
