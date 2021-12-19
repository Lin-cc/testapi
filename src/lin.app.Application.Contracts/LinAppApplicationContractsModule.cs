using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Modularity;

namespace lin.app.Application.Contracts
{
    [DependsOn(
    // typeof(LinAppApplicationModule)
    )]
    public class LinAppApplicationContractsModule : AbpModule
    {

    }
}