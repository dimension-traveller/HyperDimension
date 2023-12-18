﻿// <auto-generated />
using System;
using HyperDimension.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HyperDimension.Migrations.SQLServer.Migrations
{
    [DbContext(typeof(HyperDimensionDbContext))]
    partial class HyperDimensionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ApiToken", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("ExpiredAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastUsedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("RevokedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("ApiTokens");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ExternalProvider", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("ExternalProviders");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.Role", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.Totp", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTimeOffset>("RegistrationTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("EntityId");

                    b.ToTable("Totps");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.TotpRecoveryCode", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("TotpEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UsedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("EntityId");

                    b.HasIndex("TotpEntityId");

                    b.ToTable("TotpRecoveryCodes");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.User", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("FailedAccessAttempts")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("LockoutEndAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TotpEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("TwoFactorEmailEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorTotpEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId");

                    b.HasIndex("TotpEntityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.WebAuthn", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AaGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CredType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("CredentialId")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTimeOffset>("RegDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("SignatureCounter")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("UserHandle")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("WebAuthnDevices");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Security.Token", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BindTo")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ExpiredAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Usage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EntityId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FriendlyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Xml")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RolesEntityId", "UsersEntityId");

                    b.HasIndex("UsersEntityId");

                    b.ToTable("RoleUser");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ApiToken", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.User", "User")
                        .WithMany("ApiTokens")
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ExternalProvider", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.User", "User")
                        .WithMany("ExternalProviders")
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.TotpRecoveryCode", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.Totp", null)
                        .WithMany("RecoveryCodes")
                        .HasForeignKey("TotpEntityId");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.User", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.Totp", "Totp")
                        .WithMany()
                        .HasForeignKey("TotpEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Totp");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.WebAuthn", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.User", "User")
                        .WithMany("WebAuthnDevices")
                        .HasForeignKey("UserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("HyperDimension.Domain.Entities.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HyperDimension.Domain.Entities.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UsersEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.Totp", b =>
                {
                    b.Navigation("RecoveryCodes");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.User", b =>
                {
                    b.Navigation("ApiTokens");

                    b.Navigation("ExternalProviders");

                    b.Navigation("WebAuthnDevices");
                });
#pragma warning restore 612, 618
        }
    }
}