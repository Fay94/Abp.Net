using Abp.Net.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Abp.Net;

[DependsOn(
    typeof(NetEntityFrameworkCoreTestModule)
    )]
public class NetDomainTestModule : AbpModule
{

}
