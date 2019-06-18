using System.Collections.Generic;

namespace DRCDesignerWebApplication.Models
{
    public class Authorization
    {
        public int Id { get; set; }
        public int DrcCardId { get; set; }
        public string OperationName { get; set; }

        public virtual ICollection<Role> Fields { get; set; }
        public virtual DrcCard DrcCard { get; set; }
     
    }
}