using Volo.Abp.Modularity;

namespace Abp.Net;

[DependsOn(
    typeof(NetApplicationModule),
    typeof(NetDomainTestModule)
    )]
public class NetApplicationTestModule : AbpModule
{

}
