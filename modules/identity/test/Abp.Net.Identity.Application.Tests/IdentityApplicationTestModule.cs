using Volo.Abp.Modularity;

namespace Abp.Net.Identity;

[DependsOn(
    typeof(IdentityApplicationModule),
    typeof(IdentityDomainTestModule)
    )]
public class IdentityApplicationTestModule : AbpModule
{

}
