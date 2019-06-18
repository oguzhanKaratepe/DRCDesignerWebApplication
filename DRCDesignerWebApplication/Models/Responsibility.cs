namespace DRCDesignerWebApplication.Models
{
    public class Responsibility
    {
        public int Id { get; set; }
        public int DrcCardId { get; set; }
        public string Title { get; set; }
        public string ResponsibilityDefinition { get; set; }
        public bool IsMandatory { get; set; }
        public virtual DrcCard Drc { get; set; }
    }
}