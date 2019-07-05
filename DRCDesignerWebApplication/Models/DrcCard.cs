using System.Collections.Generic;

namespace DRCDesignerWebApplication.Models
{
    public class DrcCard
    {
        public int Id { get; set; }
        public int SubdomainId { get; set; }
        public string DrcCardName { get; set; }
        public int Order { get; set; }
        public virtual Subdomain Subdomain { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Responsibility> Responsibilities { get; set; }
        public virtual ICollection<Authorization> Authorizations { get; set; }
    }
}