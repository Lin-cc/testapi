using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lin.app.Domain;
using lin.app.EntityFrameworkCore.EntityframeworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace lin.app.Application.EntityFrameworkCore
{
    [DependsOn(
        typeof(LinAppDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class LinAppEntityFrameworkModule : AbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddAbpDbContext<LinAppDbContext>(option =>
            {
                //需要注册默认的仓储
                option.AddDefaultRepositories<LinAppDbContext>(true);
            });
            Configure<AbpDbContextOptions>(option =>
            {
                option.UseMySQL();
            });
        }
    }
}