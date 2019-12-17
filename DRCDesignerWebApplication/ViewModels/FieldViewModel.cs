

using System.ComponentModel.DataAnnotations;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class FieldViewModel
    {
        public int Id { get; set; }
        [Required]
        public string AttributeName { get; set; }
        public FieldType Type { get; set; }
        public int DrcCardId { get; set; }
        public int? CollaborationId { get; set; }
        [JsonIgnore]
        public DrcCard CollaborationCard{ get; set; }
        [JsonIgnore]
        public DrcCard DrcCard { get; set; }
        public EMeasurementTypes MeasurementType { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public bool Nullable { get; set; }
        public string DefaultValue { get; set; }
        public bool MoreDetails { get; set; }
        public bool IsShadowField { get; set; }
        public double? MinValue { get; set; }
       public double? MaxValue { get; set; }

       public bool Unique { get; set; }
        public int? MinLength { get; set; }
    
        public int? MaxLength { get; set; }
       public bool CreditCard { get; set; }
       public bool Required { get; set; }
       public string RegularExpression { get; set; }
       public string EnumValues { get; set; }
    }
}
