using System;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace lin.app.Domain.Shared
{
    [DependsOn(
        typeof(AbpIdentityDomainSharedModule)
       
    )]
    public class LinAppDomainSharedModule : AbpModule
    {

    }
}
