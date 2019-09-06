using System.Collections.Generic;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class ResponsibilityViewModel
    {
        public ResponsibilityViewModel()
        {
            ShadowCardIds=new int[0];
            ResponsibilityCollaborationCards=new List<DrcCard>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int PriorityOrder { get; set; }
        public int DrcCardId { get; set; }
        [JsonIgnore]
        public virtual DrcCard DrcCard { get; set; }
        public string ResponsibilityDefinition { get; set; }
        public bool IsMandatory { get; set; }
        public int[] ShadowCardIds { get; set; }
        [JsonIgnore]
        public virtual IList<DrcCard> ResponsibilityCollaborationCards { get; set; }
    }
}
