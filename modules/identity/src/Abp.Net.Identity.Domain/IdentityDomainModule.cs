using Volo.Abp.Domain;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;

namespace Abp.Net.Identity;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpIdentityDomainModule),
    typeof(IdentityDomainSharedModule)
)]
public class IdentityDomainModule : AbpModule
{
}