using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.Models
{
    public class DrcCard
    {
    
        public int Id { get; set; }
        public int SubdomainId { get; set; }
        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }
        public int? MainCardId { get; set; }
        [Required(ErrorMessage = "Card Name is required")]
        [DisplayName("Card Name")]
        public string DrcCardName { get; set; }
        public int Order { get; set; }
     
        [JsonIgnore]
        public virtual ICollection<Field> Fields { get; set; }
        [JsonIgnore]
        public virtual ICollection<Responsibility> Responsibilities { get; set; }
        [JsonIgnore]
        public virtual ICollection<Authorization> Authorizations { get; set; }

 
    }
}