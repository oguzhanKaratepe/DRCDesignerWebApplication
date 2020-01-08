using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DRCDesignerWebApplication.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttributeName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    MeasurementType = table.Column<int>(nullable: true),
                    ItemName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Nullable = table.Column<bool>(nullable: false),
                    DefaultValue = table.Column<string>(nullable: true),
                    MinValue = table.Column<double>(nullable: true),
                    MaxValue = table.Column<double>(nullable: true),
                    MinLength = table.Column<int>(nullable: true),
                    MaxLength = table.Column<int>(nullable: true),
                    CreditCard = table.Column<bool>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    Unique = table.Column<bool>(nullable: false),
                    RegularExpression = table.Column<string>(nullable: true),
                    EnumValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Responsibilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PriorityOrder = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ResponsibilityDefinition = table.Column<string>(nullable: true),
                    IsMandatory = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsibilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    IsGlobal = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subdomains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubdomainName = table.Column<string>(nullable: false),
                    SubdomainNamespace = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subdomains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubdomainVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubdomainId = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<string>(nullable: true),
                    EditLock = table.Column<bool>(nullable: false),
                    SourceVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubdomainVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubdomainVersions_SubdomainVersions_SourceVersionId",
                        column: x => x.SourceVersionId,
                        principalTable: "SubdomainVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubdomainVersions_Subdomains_SubdomainId",
                        column: x => x.SubdomainId,
                        principalTable: "Subdomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrcCards",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubdomainVersionId = table.Column<int>(nullable: false),
                    MainCardId = table.Column<int>(nullable: true),
                    DrcCardName = table.Column<string>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Definition = table.Column<string>(nullable: true),
                    SecurityCriticalOption = table.Column<int>(nullable: false),
                    DeleteBehaviorOption = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrcCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrcCards_SubdomainVersions_SubdomainVersionId",
                        column: x => x.SubdomainVersionId,
                        principalTable: "SubdomainVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubdomainVersionReferences",
                columns: table => new
                {
                    SubdomainVersionId = table.Column<int>(nullable: false),
                    ReferencedVersionId = table.Column<int>(nullable: false),
                    ReferencedSubdomainVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubdomainVersionReferences", x => new { x.SubdomainVersionId, x.ReferencedVersionId });
                    table.ForeignKey(
                        name: "FK_SubdomainVersionReferences_SubdomainVersions_ReferencedSubdomainVersionId",
                        column: x => x.ReferencedSubdomainVersionId,
                        principalTable: "SubdomainVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubdomainVersionReferences_SubdomainVersions_SubdomainVersionId",
                        column: x => x.SubdomainVersionId,
                        principalTable: "SubdomainVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubdomainVersionRoles",
                columns: table => new
                {
                    SubdomainVersionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubdomainVersionRoles", x => new { x.SubdomainVersionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SubdomainVersionRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubdomainVersionRoles_SubdomainVersions_SubdomainVersionId",
                        column: x => x.SubdomainVersionId,
                        principalTable: "SubdomainVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DrcCardId = table.Column<int>(nullable: false),
                    OperationName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_DrcCards_DrcCardId",
                        column: x => x.DrcCardId,
                        principalTable: "DrcCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrcCardFields",
                columns: table => new
                {
                    DrcCardId = table.Column<int>(nullable: false),
                    FieldId = table.Column<int>(nullable: false),
                    IsRelationCollaboration = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrcCardFields", x => new { x.DrcCardId, x.FieldId });
                    table.ForeignKey(
                        name: "FK_DrcCardFields_DrcCards_DrcCardId",
                        column: x => x.DrcCardId,
                        principalTable: "DrcCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrcCardFields_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrcCardResponsibilities",
                columns: table => new
                {
                    DrcCardId = table.Column<int>(nullable: false),
                    ResponsibilityId = table.Column<int>(nullable: false),
                    IsRelationCollaboration = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrcCardResponsibilities", x => new { x.DrcCardId, x.ResponsibilityId });
                    table.ForeignKey(
                        name: "FK_DrcCardResponsibilities_DrcCards_DrcCardId",
                        column: x => x.DrcCardId,
                        principalTable: "DrcCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrcCardResponsibilities_Responsibilities_ResponsibilityId",
                        column: x => x.ResponsibilityId,
                        principalTable: "Responsibilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationRoles",
                columns: table => new
                {
                    AuthorizationId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationRoles", x => new { x.RoleId, x.AuthorizationId });
                    table.UniqueConstraint("AK_AuthorizationRoles_AuthorizationId_RoleId", x => new { x.AuthorizationId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AuthorizationRoles_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorizationRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_DrcCardId",
                table: "Authorizations",
                column: "DrcCardId");

            migrationBuilder.CreateIndex(
                name: "IX_DrcCardFields_FieldId",
                table: "DrcCardFields",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_DrcCardResponsibilities_ResponsibilityId",
                table: "DrcCardResponsibilities",
                column: "ResponsibilityId");

            migrationBuilder.CreateIndex(
                name: "IX_DrcCards_SubdomainVersionId",
                table: "DrcCards",
                column: "SubdomainVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubdomainVersionReferences_ReferencedSubdomainVersionId",
                table: "SubdomainVersionReferences",
                column: "ReferencedSubdomainVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubdomainVersionRoles_RoleId",
                table: "SubdomainVersionRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubdomainVersions_SourceVersionId",
                table: "SubdomainVersions",
                column: "SourceVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubdomainVersions_SubdomainId",
                table: "SubdomainVersions",
                column: "SubdomainId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationRoles");

            migrationBuilder.DropTable(
                name: "DrcCardFields");

            migrationBuilder.DropTable(
                name: "DrcCardResponsibilities");

            migrationBuilder.DropTable(
                name: "SubdomainVersionReferences");

            migrationBuilder.DropTable(
                name: "SubdomainVersionRoles");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Responsibilities");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "DrcCards");

            migrationBuilder.DropTable(
                name: "SubdomainVersions");

            migrationBuilder.DropTable(
                name: "Subdomains");
        }
    }
}
