using System;
using lin.app.Application.Contracts;
using lin.app.Application.Contracts.Services;
using lin.app.Application.ServicesImp;
using lin.app.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace lin.app.Application
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(LinAppDomainModule),
        typeof(LinAppApplicationContractsModule)
    )]
    public class LinAppApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //context.Services.AddAutoMapperObjectMapper<LinAppApplicationModule>();
            context.Services.AddTransient<IChatUserApplicationService, ChatUserService>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<LinAppApplicationModule>(validate: true);
            });
        }
    }
}
