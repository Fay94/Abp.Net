using Abp.Net.Identity.Localization;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Abp.Net.Identity;

[DependsOn(
    typeof(AbpIdentityDomainSharedModule),
    typeof(AbpValidationModule)
)]
public class IdentityDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<IdentityDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<IdentityResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/Identity");

            // 扩展现有资源，但是不能和原有资源virtualPath相同，不然会完全覆盖
            options.Resources
                .Get<Volo.Abp.Identity.Localization.IdentityResource>()
                .AddVirtualJson("/Localization/Identity");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Identity", typeof(IdentityResource));
        });
    }
}
