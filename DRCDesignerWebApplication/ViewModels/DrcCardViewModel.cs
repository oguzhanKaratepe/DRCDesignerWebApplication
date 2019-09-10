using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DRCDesigner.Entities.Concrete;
using Newtonsoft.Json;

namespace DRCDesignerWebApplication.ViewModels
{
    public class DrcCardViewModel
    {
        public DrcCardViewModel()
        {
            Authorizations=new List<AuthorizationViewModel>();
            Responsibilities=new List<ResponsibilityViewModel>();
            Fields=new List<FieldViewModel>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Subdomain is required")]
        [DisplayName("Subdomain")]
        public int SubdomainId { get; set; }
        [JsonIgnore]
        public virtual Subdomain Subdomain { get; set; }
        public int? MainCardId { get; set; }
        [Required(ErrorMessage = "Document Name is required")]
        [DisplayName("Document Name")]
        public string DrcCardName { get; set; }
        public int Order { get; set; }
        public string SourceDrcCardPath { get; set; }
        [JsonIgnore]
        public virtual ICollection<FieldViewModel> Fields { get; set; }
        [JsonIgnore]
        public virtual IList<ResponsibilityViewModel> Responsibilities { get; set; }
        [JsonIgnore]
        public virtual ICollection<AuthorizationViewModel> Authorizations { get; set; }
  

    }
}
