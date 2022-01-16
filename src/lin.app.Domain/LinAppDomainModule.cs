using System;
using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace lin.app.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule),
         typeof(AbpIdentityDomainModule)
    )]
    public class LinAppDomainModule : AbpModule
    {
    }
}
