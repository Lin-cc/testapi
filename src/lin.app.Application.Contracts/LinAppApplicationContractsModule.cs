using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace lin.app.Application.Contracts
{
    [DependsOn(
    // typeof(LinAppApplicationModule)
    typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityApplicationContractsModule)
    )]
    public class LinAppApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {

        }
    }
}