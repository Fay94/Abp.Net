using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Abp.Net.Identity;

[DependsOn(
    typeof(IdentityDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class IdentityApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    IdentityModuleExtensionConsts.ModuleName,
                    IdentityModuleExtensionConsts.EntityNames.OrganizationUnit,
                    getApiTypes: new[] { typeof(OrganizationUnitDto) },
                    createApiTypes: new[] { typeof(OrganizationUnitCreateDto) },
                    updateApiTypes: new[] { typeof(OrganizationUnitUpdateDto) }
                );
        });
    }
}
