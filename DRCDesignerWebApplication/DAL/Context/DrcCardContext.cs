using DRCDesignerWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DRCDesignerWebApplication.DAL.Context
{
    public class DrcCardContext : DbContext
    {

        public DrcCardContext(DbContextOptions<DrcCardContext> context) : base(context)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DrcCardResponsibility>().HasKey(sc => new { sc.DrcCardID, sc.ResponsibilityID });
            modelBuilder.Entity<AuthorizationRole>().HasKey(sc => new { sc.AuthorizationId, sc.RoleId });

            modelBuilder.Entity<DrcCard>()
                .HasMany(c => c.Fields)
                .WithOne(s=>s.DrcCard)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<DrcCard>()
                .HasMany(c => c.Authorizations)
                .WithOne(s => s.DrcCard)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<DrcCard>()
                .HasMany(c => c.Responsibilities)
                .WithOne(s => s.DrcCard)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

        }


        public DbSet<Subdomain> Subdomains { get; set; }
        public DbSet<DrcCard> DrcCards { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }
        public DbSet<DrcCardResponsibility> DrcCardResponsibilities { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<AuthorizationRole> AuthorizationRoles { get; set; }
      

    }
}
