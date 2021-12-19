using System;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace lin.app.Domain
{
    [DependsOn(
        typeof(AbpDddDomainModule)
    )]
    public class LinAppDomainModule : AbpModule
    {
    }
}
