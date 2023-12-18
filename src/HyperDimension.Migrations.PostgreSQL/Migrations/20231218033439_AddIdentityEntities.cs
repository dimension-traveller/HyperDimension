using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HyperDimension.Migrations.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FriendlyName = table.Column<string>(type: "text", nullable: true),
                    Xml = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Permissions = table.Column<List<string>>(type: "text[]", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    BindTo = table.Column<Guid>(type: "uuid", nullable: false),
                    Usage = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "Totps",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<byte[]>(type: "bytea", nullable: false),
                    RegistrationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totps", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "TotpRecoveryCodes",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TotpEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotpRecoveryCodes", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_TotpRecoveryCodes_Totps_TotpEntityId",
                        column: x => x.TotpEntityId,
                        principalTable: "Totps",
                        principalColumn: "EntityId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: false),
                    TwoFactorEmailEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorTotpEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    FailedAccessAttempts = table.Column<int>(type: "integer", nullable: false),
                    LockoutEndAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TotpEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_Users_Totps_TotpEntityId",
                        column: x => x.TotpEntityId,
                        principalTable: "Totps",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiTokens",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUsedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiTokens", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ApiTokens_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExternalProviders",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProviderId = table.Column<string>(type: "text", nullable: false),
                    UserIdentifier = table.Column<string>(type: "text", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalProviders", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_ExternalProviders_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesEntityId, x.UsersEntityId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesEntityId",
                        column: x => x.RolesEntityId,
                        principalTable: "Roles",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UsersEntityId",
                        column: x => x.UsersEntityId,
                        principalTable: "Users",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebAuthnDevices",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CredentialId = table.Column<byte[]>(type: "bytea", nullable: false),
                    PublicKey = table.Column<byte[]>(type: "bytea", nullable: false),
                    UserHandle = table.Column<byte[]>(type: "bytea", nullable: false),
                    SignatureCounter = table.Column<long>(type: "bigint", nullable: false),
                    CredType = table.Column<string>(type: "text", nullable: false),
                    RegDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AaGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebAuthnDevices", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_WebAuthnDevices_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokens_UserEntityId",
                table: "ApiTokens",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalProviders_UserEntityId",
                table: "ExternalProviders",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UsersEntityId",
                table: "RoleUser",
                column: "UsersEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TotpRecoveryCodes_TotpEntityId",
                table: "TotpRecoveryCodes",
                column: "TotpEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TotpEntityId",
                table: "Users",
                column: "TotpEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_WebAuthnDevices_UserEntityId",
                table: "WebAuthnDevices",
                column: "UserEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiTokens");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "ExternalProviders");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "TotpRecoveryCodes");

            migrationBuilder.DropTable(
                name: "WebAuthnDevices");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Totps");
        }
    }
}
