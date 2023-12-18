﻿// <auto-generated />
using System;
using HyperDimension.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HyperDimension.Migrations.SQLite.Migrations
{
    [DbContext(typeof(HyperDimensionDbContext))]
    [Migration("20231218033443_AddIdentityEntities")]
    partial class AddIdentityEntities
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ApiToken", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("ExpiredAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("LastUsedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("RevokedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("ApiTokens");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.ExternalProvider", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserIdentifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("ExternalProviders");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.Role", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.Totp", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Key")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<DateTimeOffset>("RegistrationTime")
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.ToTable("Totps");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.TotpRecoveryCode", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TotpEntityId")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("UsedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.HasIndex("TotpEntityId");

                    b.ToTable("TotpRecoveryCodes");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.User", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FailedAccessAttempts")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEndAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TotpEntityId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEmailEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("TwoFactorTotpEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.HasIndex("TotpEntityId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Identity.WebAuthn", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AaGuid")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CredType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("CredentialId")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PublicKey")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<DateTimeOffset>("RegDate")
                        .HasColumnType("TEXT");

                    b.Property<uint>("SignatureCounter")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserEntityId")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("UserHandle")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("EntityId");

                    b.HasIndex("UserEntityId");

                    b.ToTable("WebAuthnDevices");
                });

            modelBuilder.Entity("HyperDimension.Domain.Entities.Security.Token", b =>
                {
                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BindTo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ExpiredAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Usage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EntityId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FriendlyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Xml")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<Guid>("RolesEntityId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UsersEntityId")
                        .HasColumnType("TEXT");

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
