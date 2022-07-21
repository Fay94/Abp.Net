using Abp.Net.Identity;
using EasyAbp.Abp.SettingUi;
using EasyAbp.FileManagement;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.VirtualFileSystem;

namespace Abp.Net;

[DependsOn(
    typeof(NetApplicationContractsModule),
    typeof(AbpAccountHttpApiClientModule),
    typeof(IdentityHttpApiClientModule),
    typeof(AbpPermissionManagementHttpApiClientModule),
    typeof(AbpTenantManagementHttpApiClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
    [DependsOn(typeof(AbpSettingUiHttpApiClientModule))]
    [DependsOn(typeof(FileManagementHttpApiClientModule))]
public class NetHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(NetApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<NetHttpApiClientModule>();
        });
    }
}
