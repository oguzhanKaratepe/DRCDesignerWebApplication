using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DRCDesigner.Core.Entities;
using Newtonsoft.Json;

namespace DRCDesigner.Entities.Concrete
{
    public class DrcCard : IEntity
    {
    
        public int Id { get; set; }
        public int SubdomainVersionId { get; set; }
        [JsonIgnore]
        public virtual SubdomainVersion SubdomainVersion { get; set; }
        public int? MainCardId { get; set; }
        [Required(ErrorMessage = "Card Name is required")]
        [DisplayName("Card Name")]
        public string DrcCardName { get; set; }
        public int Order { get; set; }

        public ESecurityCriticalOptions SecurityCriticalOption { get; set; }

        public EDeleteBehaviorOptions DeleteBehaviorOption { get; set; }
     
        [JsonIgnore]
        public virtual ICollection<DrcCardField> DrcCardFields { get; set; }
        [JsonIgnore]
        public virtual IList<DrcCardResponsibility> DrcCardResponsibilities{ get; set; }
        [JsonIgnore]
        public virtual ICollection<Authorization> Authorizations { get; set; }

 
    }
}