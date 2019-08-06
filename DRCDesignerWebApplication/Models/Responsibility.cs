using System.Collections.Generic;

namespace DRCDesignerWebApplication.Models
{
    public class Responsibility
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DrcCardID { get; set; }
        public virtual DrcCard DrcCard { get; set; }
        public string ResponsibilityDefinition { get; set; }
        public bool IsMandatory { get; set; }
        public virtual ICollection<DrcCard> ShadowCards { get; set; }

    }
}