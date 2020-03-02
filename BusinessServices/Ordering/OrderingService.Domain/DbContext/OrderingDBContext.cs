﻿using ServiceCommon;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OrderingService.Domain.Entities;
using ServiceCommon.Models;

namespace ProductService.Domain
{
    /// <summary>
    /// Add-Migration 20191009 -Context OrderingDBContext
    /// Update-Database -Context OrderingDBContext
    /// </summary>
    public class OrderingDBContext : DbContext
    {
        private readonly UserInfo currentUserInfo;
        private readonly UserPermission currentUserPermission;
        public OrderingDBContext(DbContextOptions<OrderingDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            if (httpContextAccessor.HttpContext != null)
            {
                currentUserInfo = httpContextAccessor.HttpContext.Items["CurrentUserInfo"] as UserInfo;
                currentUserPermission = httpContextAccessor.HttpContext.Items["CurrentUserPermission"] as UserPermission;
            }
            //currentUserInfo = new UserInfo();
            //currentUserPermission = new UserPermission();
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Recycle> Recycles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>().HasKey(p => new { p.TenantCode, p.ID });
            builder.Entity<Order>().HasIndex(p => p.TenantCode);
            builder.Entity<Order>().HasIndex(p => new { p.TenantCode, p.OrderCode }).IsUnique(true);

            builder.Entity<Recycle>().HasKey(p => new { p.TenantCode, p.ID });
            builder.Entity<Recycle>().HasIndex(p => p.TenantCode);

            foreach (var entityType in GetBaseEntityTypes(builder))
            {
                GlobalTenantQueryMethodInfo.MakeGenericMethod(entityType)
                 .Invoke(this, new object[] { builder });
            }

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            UpdateCommonFileds();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateCommonFileds();
            return base.SaveChangesAsync(cancellationToken);
        }


        /// <summary>
        /// 数据新增、更新前更新公共字段
        /// </summary>
        private void UpdateCommonFileds()
        {
            var nowTime = DateTimeOffset.UtcNow;
            var deleteBatchID = Guid.NewGuid();
            foreach (var entry in this.ChangeTracker.Entries<BaseEntity>().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                var entity = entry.Entity;
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entity.TenantCode == "")
                            entity.TenantCode = currentUserInfo.TenantCode;
                        if (entity.OwnerScopeCode == "")
                            entity.OwnerScopeCode = currentUserPermission.ScopeCode;
                        entity.CreateIn = nowTime;
                        entity.CreatedBy = currentUserInfo.UserName;
                        break;
                    case EntityState.Modified:
                        entity.UpdateIn = nowTime;
                        entity.UpdatedBy = currentUserInfo.UserName;
                        break;
                    case EntityState.Deleted:
                        Recycle recycle = new Recycle()
                        {
                            TenantCode = currentUserInfo.TenantCode,
                            ID = Guid.NewGuid(),
                            CreateIn = nowTime,
                            CreatedBy = currentUserInfo.UserName,
                            TableName = entity.GetType().Name,
                            RowKey = entity.ID,
                            RowData = JsonConvert.SerializeObject(entity),
                            DeleteBatchID = deleteBatchID
                        };
                        Recycles.Add(recycle);
                        break;
                }
            }
            this.ChangeTracker.DetectChanges();
        }

        #region 多租户全局查询过滤
        private static IList<Type> _baseEntityTypesCache;
        private static IList<Type> GetBaseEntityTypes(ModelBuilder builder)
        {
            if (_baseEntityTypesCache != null)
                return _baseEntityTypesCache.ToList();
            _baseEntityTypesCache = (from t in builder.Model.GetEntityTypes()
                                     where t.ClrType.BaseType == typeof(BaseEntity)
                                     select t.ClrType).ToList();
            return _baseEntityTypesCache;
        }
        static readonly MethodInfo GlobalTenantQueryMethodInfo = typeof(OrderingDBContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                       .Single(t => t.IsGenericMethod && t.Name == "SetGlobalTenantQuery");
        public void SetGlobalTenantQuery<T>(ModelBuilder builder) where T : BaseEntity
        {
            builder.Entity<T>().HasQueryFilter(e => e.TenantCode == currentUserInfo.TenantCode);
            //数据权限Scope全局查询过滤
            if (currentUserPermission != null && currentUserPermission.AllowScopeCodes != null && currentUserPermission.AllowScopeCodes.Count() > 0)
                builder.Entity<T>().HasQueryFilter(e => currentUserPermission.AllowScopeCodes.Contains(e.OwnerScopeCode));
        }
        #endregion

        
    }
}
