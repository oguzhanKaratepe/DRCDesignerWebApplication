using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DRCDesignerWebApplication.Models
{
    public class Role
    {
        public int Id{ get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Role Name")]
        public string RoleName{ get; set; }

        public int? SubdomainId { get; set; }
        public virtual Subdomain Subdomain { get; set; }
    }
}