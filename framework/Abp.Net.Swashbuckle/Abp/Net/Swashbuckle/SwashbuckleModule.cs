using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Abp.Net.Swashbuckle
{
    [DependsOn(typeof(AbpSwashbuckleModule))]
    public class SwashbuckleModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Transient<ISwaggerHtmlResolver, SwaggerHtmlResolver>());
        }
    }
}
