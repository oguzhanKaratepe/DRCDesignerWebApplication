﻿// <auto-generated />
using System;
using DRCDesigner.DataAccess.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DRCDesignerWebApplication.Migrations
{
    [DbContext(typeof(DrcCardContext))]
    [Migration("20200109203836_SubdomainVersionUpdated")]
    partial class SubdomainVersionUpdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Authorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DrcCardId");

                    b.Property<string>("OperationName");

                    b.HasKey("Id");

                    b.HasIndex("DrcCardId");

                    b.ToTable("Authorizations");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.AuthorizationRole", b =>
                {
                    b.Property<int>("RoleId");

                    b.Property<int>("AuthorizationId");

                    b.HasKey("RoleId", "AuthorizationId");

                    b.HasAlternateKey("AuthorizationId", "RoleId");

                    b.ToTable("AuthorizationRoles");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition");

                    b.Property<int>("DeleteBehaviorOption");

                    b.Property<string>("DrcCardName")
                        .IsRequired();

                    b.Property<int?>("MainCardId");

                    b.Property<int>("Order");

                    b.Property<int>("SecurityCriticalOption");

                    b.Property<int>("SubdomainVersionId");

                    b.HasKey("Id");

                    b.HasIndex("SubdomainVersionId");

                    b.ToTable("DrcCards");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCardField", b =>
                {
                    b.Property<int>("DrcCardId");

                    b.Property<int>("FieldId");

                    b.Property<bool>("IsRelationCollaboration");

                    b.HasKey("DrcCardId", "FieldId");

                    b.HasIndex("FieldId");

                    b.ToTable("DrcCardFields");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCardResponsibility", b =>
                {
                    b.Property<int>("DrcCardId");

                    b.Property<int>("ResponsibilityId");

                    b.Property<bool>("IsRelationCollaboration");

                    b.HasKey("DrcCardId", "ResponsibilityId");

                    b.HasIndex("ResponsibilityId");

                    b.ToTable("DrcCardResponsibilities");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AttributeName");

                    b.Property<bool>("CreditCard");

                    b.Property<string>("DefaultValue");

                    b.Property<string>("Description");

                    b.Property<string>("EnumValues");

                    b.Property<string>("ItemName");

                    b.Property<int?>("MaxLength");

                    b.Property<double?>("MaxValue");

                    b.Property<int?>("MeasurementType");

                    b.Property<int?>("MinLength");

                    b.Property<double?>("MinValue");

                    b.Property<bool>("Nullable");

                    b.Property<string>("RegularExpression");

                    b.Property<bool>("Required");

                    b.Property<int>("Type");

                    b.Property<bool>("Unique");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Responsibility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsMandatory");

                    b.Property<int>("PriorityOrder");

                    b.Property<string>("ResponsibilityDefinition");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Responsibilities");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsGlobal");

                    b.Property<string>("RoleName");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Subdomain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("SubdomainName")
                        .IsRequired();

                    b.Property<string>("SubdomainNamespace");

                    b.HasKey("Id");

                    b.ToTable("Subdomains");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DexmoVersion");

                    b.Property<bool>("EditLock");

                    b.Property<int?>("SourceVersionId");

                    b.Property<int>("SubdomainId");

                    b.Property<string>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("SourceVersionId");

                    b.HasIndex("SubdomainId");

                    b.ToTable("SubdomainVersions");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersionReference", b =>
                {
                    b.Property<int>("SubdomainVersionId");

                    b.Property<int>("ReferencedVersionId");

                    b.Property<int?>("ReferencedSubdomainVersionId");

                    b.HasKey("SubdomainVersionId", "ReferencedVersionId");

                    b.HasIndex("ReferencedSubdomainVersionId");

                    b.ToTable("SubdomainVersionReferences");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersionRole", b =>
                {
                    b.Property<int>("SubdomainVersionId");

                    b.Property<int>("RoleId");

                    b.HasKey("SubdomainVersionId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("SubdomainVersionRoles");
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.Authorization", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.DrcCard", "DrcCard")
                        .WithMany("Authorizations")
                        .HasForeignKey("DrcCardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.AuthorizationRole", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.Authorization", "Authorization")
                        .WithMany("AuthorizationRoles")
                        .HasForeignKey("AuthorizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DRCDesigner.Entities.Concrete.Role", "Role")
                        .WithMany("AuthorizationRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCard", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.SubdomainVersion", "SubdomainVersion")
                        .WithMany("DRCards")
                        .HasForeignKey("SubdomainVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCardField", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.DrcCard", "DrcCard")
                        .WithMany("DrcCardFields")
                        .HasForeignKey("DrcCardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DRCDesigner.Entities.Concrete.Field", "Field")
                        .WithMany("DrcCardFields")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.DrcCardResponsibility", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.DrcCard", "DrcCard")
                        .WithMany("DrcCardResponsibilities")
                        .HasForeignKey("DrcCardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DRCDesigner.Entities.Concrete.Responsibility", "Responsibility")
                        .WithMany("DrcCardResponsibilities")
                        .HasForeignKey("ResponsibilityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersion", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.SubdomainVersion", "SourceSubdomainVersion")
                        .WithMany()
                        .HasForeignKey("SourceVersionId");

                    b.HasOne("DRCDesigner.Entities.Concrete.Subdomain", "Subdomain")
                        .WithMany("SubdomainVersions")
                        .HasForeignKey("SubdomainId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersionReference", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.SubdomainVersion", "ReferencedSubdomainVersion")
                        .WithMany()
                        .HasForeignKey("ReferencedSubdomainVersionId");

                    b.HasOne("DRCDesigner.Entities.Concrete.SubdomainVersion", "SubdomainVersion")
                        .WithMany("ReferencedSubdomainVersions")
                        .HasForeignKey("SubdomainVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DRCDesigner.Entities.Concrete.SubdomainVersionRole", b =>
                {
                    b.HasOne("DRCDesigner.Entities.Concrete.Role", "Role")
                        .WithMany("SubdomainVersionRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DRCDesigner.Entities.Concrete.SubdomainVersion", "SubdomainVersion")
                        .WithMany("SubdomainVersionRoles")
                        .HasForeignKey("SubdomainVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
