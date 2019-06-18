using System.Collections.Generic;

namespace DRCDesignerWebApplication.Models
{
    public class Subdomain:BaseEntity
    {
        public int Id { get; set; }
        public int DrcProjectId { get; set; } 
        public string SubdomainName { get; set; }

        public virtual DrcProject DrcProject { get; set; }
        public virtual ICollection<DrcCard> DRCards { get; set; }
        public virtual ICollection<Role> SubdomainRoles { get; set; }
    }
}