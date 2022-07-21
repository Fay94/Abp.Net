using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace Abp.Net.Swashbuckle.Export
{
    [DependsOn(typeof(AbpSwashbuckleModule))]
    public class SwaggerExportModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SwaggerExportModule>();
            });
            context.Services.AddScoped<SwaggerGenerator>();
        }
    }
}