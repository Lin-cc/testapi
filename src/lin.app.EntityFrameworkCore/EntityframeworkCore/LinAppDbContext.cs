using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lin.app.Domain.Entitys;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace lin.app.EntityFrameworkCore.EntityframeworkCore
{
    public class LinAppDbContext : AbpDbContext<LinAppDbContext>, IEfCoreDbContext
    {
  
        public LinAppDbContext(DbContextOptions<LinAppDbContext> options) : base(options)
        {

        }
       
        public DbSet<ChatUser> ChatUsers { get; set; }

    }
}