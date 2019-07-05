using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DRCDesignerWebApplication.Models
{
    public class Subdomain
    {


        public Subdomain()
        {
            DRCards = new List<DrcCard>();
            SubdomainRoles = new List<Role>();
        }
        [Key]
        public int Id { get; set; }    
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Full Name")]
        public string SubdomainName { get; set; }
        public virtual ICollection<DrcCard> DRCards { get; set; }
        public virtual ICollection<Role> SubdomainRoles { get; set; }
    }
}