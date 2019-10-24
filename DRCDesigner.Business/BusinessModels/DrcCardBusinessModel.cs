using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesigner.Business.BusinessModels
{
   public class DrcCardBusinessModel
    {
        public DrcCardBusinessModel()
        {
         
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Subdomain is required")]
        [DisplayName("Subdomain")]
        public int SubdomainVersionId { get; set; }
        [JsonIgnore]
        public virtual SubdomainVersion SubdomainVersion { get; set; }
        public int? MainCardId { get; set; }
        [Required(ErrorMessage = "Document Name is required")]
        [DisplayName("Document Name")]
        public string DrcCardName { get; set; }
        public ESecurityCriticalOptions SecurityCriticalOption { get; set; }

        public EDeleteBehaviorOptions DeleteBehaviorOption { get; set; }

        public int Order { get; set; }
        public string SourceDrcCardPath { get; set; }
     

    }
}
