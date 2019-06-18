namespace DRCDesignerWebApplication.Models
{
    public class Role
    {
        public int Id{ get; set; }
        public string RoleName{ get; set; }
        public int SubdomainId { get; set; }
        public virtual Subdomain Subdomain { get; set; }
    }
}