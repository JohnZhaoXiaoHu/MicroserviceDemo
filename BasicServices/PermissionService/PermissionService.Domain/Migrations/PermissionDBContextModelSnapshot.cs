﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PermissionService.Domain;

namespace PermissionService.Domain.Migrations
{
    [DbContext(typeof(PermissionDBContext))]
    partial class PermissionDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("CommonLibrary.Recycle", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<Guid>("DeleteBatchID");

                    b.Property<string>("OwnerScopeCode");

                    b.Property<string>("RowData")
                        .IsRequired();

                    b.Property<Guid>("RowKey");

                    b.Property<string>("TableName")
                        .IsRequired();

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.ToTable("Recycle");
                });

            modelBuilder.Entity("PermissionService.Domain.Principal", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("OwnerScopeCode");

                    b.Property<string>("PrincipalCode")
                        .IsRequired();

                    b.Property<string>("PrincipalDesc");

                    b.Property<string>("PrincipalName")
                        .IsRequired();

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "PrincipalCode")
                        .IsUnique();

                    b.ToTable("Principal");
                });

            modelBuilder.Entity("PermissionService.Domain.Resource", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("FullResourceCode")
                        .IsRequired();

                    b.Property<string>("OwnerScopeCode");

                    b.Property<Guid>("ParentResourceID");

                    b.Property<string>("ResourceCode")
                        .IsRequired();

                    b.Property<string>("ResourceDesc");

                    b.Property<string>("ResourceName")
                        .IsRequired();

                    b.Property<int>("ResourceType");

                    b.Property<int>("SortNO");

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "ParentResourceID");

                    b.HasIndex("TenantCode", "ResourceCode")
                        .IsUnique();

                    b.ToTable("Resource");
                });

            modelBuilder.Entity("PermissionService.Domain.Role", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("OwnerScopeCode");

                    b.Property<string>("RoleCode")
                        .IsRequired();

                    b.Property<string>("RoleDesc");

                    b.Property<string>("RoleName")
                        .IsRequired();

                    b.Property<int>("RoleType");

                    b.Property<int>("SortNO");

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "RoleCode")
                        .IsUnique();

                    b.ToTable("Role");
                });

            modelBuilder.Entity("PermissionService.Domain.RoleAssignment", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("OwnerScopeCode");

                    b.Property<Guid>("PrincipalID");

                    b.Property<Guid>("RoleID");

                    b.Property<Guid>("ScopeID");

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "PrincipalID");

                    b.HasIndex("TenantCode", "RoleID");

                    b.HasIndex("TenantCode", "ScopeID");

                    b.ToTable("RoleAssignment");
                });

            modelBuilder.Entity("PermissionService.Domain.RolePermission", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("OwnerScopeCode");

                    b.Property<Guid>("ResourceID");

                    b.Property<Guid>("RoleID");

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "ResourceID");

                    b.HasIndex("TenantCode", "RoleID");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("PermissionService.Domain.Scope", b =>
                {
                    b.Property<string>("TenantCode");

                    b.Property<Guid>("ID");

                    b.Property<DateTimeOffset>("CreateIn");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("FullScopeCode")
                        .IsRequired();

                    b.Property<string>("OwnerScopeCode");

                    b.Property<Guid>("ParentScopeID");

                    b.Property<string>("ScopeCode")
                        .IsRequired();

                    b.Property<string>("ScopeDesc");

                    b.Property<string>("ScopeName")
                        .IsRequired();

                    b.Property<int>("SortNO");

                    b.Property<DateTimeOffset?>("UpdateIn");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("TenantCode", "ID");

                    b.HasIndex("TenantCode");

                    b.HasIndex("TenantCode", "ParentScopeID");

                    b.HasIndex("TenantCode", "ScopeCode")
                        .IsUnique();

                    b.ToTable("Scope");
                });

            modelBuilder.Entity("PermissionService.Domain.Resource", b =>
                {
                    b.HasOne("PermissionService.Domain.Resource", "ParentResource")
                        .WithMany("ChildrenResources")
                        .HasForeignKey("TenantCode", "ParentResourceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PermissionService.Domain.RoleAssignment", b =>
                {
                    b.HasOne("PermissionService.Domain.Principal", "Principal")
                        .WithMany("RoleAssignments")
                        .HasForeignKey("TenantCode", "PrincipalID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PermissionService.Domain.Role", "Role")
                        .WithMany("RoleAssignments")
                        .HasForeignKey("TenantCode", "RoleID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PermissionService.Domain.Scope", "Scope")
                        .WithMany("RoleAssignments")
                        .HasForeignKey("TenantCode", "ScopeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PermissionService.Domain.RolePermission", b =>
                {
                    b.HasOne("PermissionService.Domain.Resource", "Resource")
                        .WithMany("RolePermissions")
                        .HasForeignKey("TenantCode", "ResourceID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PermissionService.Domain.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("TenantCode", "RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PermissionService.Domain.Scope", b =>
                {
                    b.HasOne("PermissionService.Domain.Scope", "ParentScope")
                        .WithMany("ChildrenScopes")
                        .HasForeignKey("TenantCode", "ParentScopeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
