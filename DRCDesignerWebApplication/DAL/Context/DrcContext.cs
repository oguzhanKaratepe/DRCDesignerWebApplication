using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DRCDesignerWebApplication.DAL.Context
{
    public class DrcCardContext:DbContext
    {

  
        public DbSet<DrcProject> Projects { get; set; }
        public DbSet<Subdomain> Subdomains { get; set; }
        public DbSet<DrcCard> DrcCards { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }
        public DbSet<ResponsibilityCollaboration> ResponsibilityCollaborations { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<AuthorizationRole> AuthorizationRoles { get; set; }
        
    }
}
