using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using EasyAbp.Abp.SettingUi;
using EasyAbp.FileManagement;
using EasyAbp.Abp.Trees;
using Abp.Net.Identity;

namespace Abp.Net;

[DependsOn(
    typeof(NetDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(NetApplicationContractsModule),
    typeof(IdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
    [DependsOn(typeof(AbpSettingUiApplicationModule))]
    [DependsOn(typeof(FileManagementApplicationModule))]
    [DependsOn(typeof(AbpTreesApplicationModule))]
public class NetApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<NetApplicationModule>();
        });
    }
}
