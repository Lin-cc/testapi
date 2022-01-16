using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lin.app.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace lin.app.EntityFrameworkCore.EntityframeworkCore
{
    public class LinAppDbContext : AbpDbContext<LinAppDbContext>, IEfCoreDbContext
    {
  
        public LinAppDbContext(DbContextOptions<LinAppDbContext> options) : base(options)
        {

        }

        //Identity
        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityClaimType> ClaimTypes { get; set; }
        public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
        public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
        public DbSet<IdentityLinkUser> LinkUsers { get; set; }


        //Local Chat
        public DbSet<ChatUser> ChatUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureIdentity();
        }
    }
}