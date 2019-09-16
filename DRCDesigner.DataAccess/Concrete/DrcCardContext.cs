
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DRCDesigner.Entities.Concrete;


namespace DRCDesigner.DataAccess.Concrete
{
    public class DrcCardContext : DbContext
    {

        public DrcCardContext(DbContextOptions<DrcCardContext> context) : base(context)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DrcCardResponsibility>()
                .HasKey(pt => new { pt.DrcCardId, pt.ResponsibilityId });

            modelBuilder.Entity<DrcCardResponsibility>()
                .HasOne(pt => pt.DrcCard)
                .WithMany(p => p.DrcCardResponsibilities)
                .HasForeignKey(pt => pt.DrcCardId);

            modelBuilder.Entity<DrcCardResponsibility>()
                .HasOne(pt => pt.Responsibility)
                .WithMany(t => t.DrcCardResponsibilities)
                .HasForeignKey(pt => pt.ResponsibilityId);

            modelBuilder.Entity<DrcCardField>()
                .HasKey(pt => new { pt.DrcCardId, pt.FieldId });

            modelBuilder.Entity<DrcCardField>()
                .HasOne(pt => pt.DrcCard)
                .WithMany(p => p.DrcCardFields)
                .HasForeignKey(pt => pt.DrcCardId);

            modelBuilder.Entity<DrcCardField>()
                .HasOne(pt => pt.Field)
                .WithMany(t => t.DrcCardFields)
                .HasForeignKey(pt => pt.FieldId);

            modelBuilder.Entity<AuthorizationRole>()
                .HasKey(pt => new { pt.RoleId, pt.AuthorizationId});

            modelBuilder.Entity<AuthorizationRole>()
                .HasOne(pt => pt.Authorization)
                .WithMany(p => p.AuthorizationRoles)
                .HasForeignKey(pt => pt.AuthorizationId);

            modelBuilder.Entity<AuthorizationRole>()
                .HasOne(pt => pt.Role)
                .WithMany(t => t.AuthorizationRoles)
                .HasForeignKey(pt => pt.RoleId);


            modelBuilder.Entity<DrcCard>()
                .HasMany(c => c.Authorizations)
                .WithOne(s => s.DrcCard)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            
            modelBuilder.Entity<SubdomainVersion>()
                .HasMany(c => c.SubdomainVersionRoles)
                .WithOne(s => s.SubdomainVersion)
                .HasForeignKey(pt => pt.SubdomainVersionId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<SubdomainVersion>()
                .HasMany(c => c.SubdomainVersionRoles)
                .WithOne(s => s.SubdomainVersion)
                .HasForeignKey(pt => pt.SubdomainVersionId);

            modelBuilder.Entity<SubdomainVersion>()
                .HasMany(c => c.DRCards)
                .WithOne(s => s.SubdomainVersion)
                .HasForeignKey(pt => pt.SubdomainVersionId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            modelBuilder.Entity<Subdomain>()
                .HasMany(c => c.SubdomainVersions)
                .WithOne(s => s.Subdomain)
                .HasForeignKey(pt => pt.SubdomainId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
        }


        public DbSet<Subdomain> Subdomains { get; set; }
        public DbSet<SubdomainVersion> SubdomainVersions { get; set; }
        public DbSet<DrcCard> DrcCards { get; set; }
        public  DbSet<Responsibility> Responsibilities { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<DrcCardResponsibility> DrcCardResponsibilities { get; set; }
        public DbSet<AuthorizationRole> AuthorizationRoles { get; set; }
        public DbSet<DrcCardField> DrcCardFields { get; set; }



    }
}
