using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using EasyAbp.Abp.SettingUi;
using EasyAbp.FileManagement;
using EasyAbp.Abp.Trees;
using Abp.Net.Identity;

namespace Abp.Net;

[DependsOn(
    typeof(NetDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(IdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule)
)]
    [DependsOn(typeof(AbpSettingUiApplicationContractsModule))]
    [DependsOn(typeof(FileManagementApplicationContractsModule))]
    [DependsOn(typeof(AbpTreesApplicationContractsModule))]
public class NetApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        NetDtoExtensions.Configure();
    }
}
