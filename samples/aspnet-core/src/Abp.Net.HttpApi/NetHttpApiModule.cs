using Abp.Net.Identity;
using Abp.Net.Localization;
using EasyAbp.Abp.SettingUi;
using EasyAbp.Abp.Trees;
using EasyAbp.FileManagement;
using Localization.Resources.AbpUi;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace Abp.Net;

[DependsOn(
    typeof(NetApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(IdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
    [DependsOn(typeof(AbpSettingUiHttpApiModule))]
    [DependsOn(typeof(FileManagementHttpApiModule))]
    [DependsOn(typeof(AbpTreesHttpApiModule))]
public class NetHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<NetResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
