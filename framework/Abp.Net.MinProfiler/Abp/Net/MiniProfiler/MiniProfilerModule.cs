using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace Abp.Net.MiniProfiler
{
    [DependsOn(
        typeof(AbpSwashbuckleModule)
        )]
    public class MiniProfilerModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            var miniProfilerOptions = context.Services.ExecutePreConfiguredActions<MiniProfilerOptions>();
            if (miniProfilerOptions.IsEnabled)
            {
                if (miniProfilerOptions.InjectSwagger)
                {
                    context.Services.Replace(ServiceDescriptor.Transient<ISwaggerHtmlResolver, MiniProfilerHtmlResolver>());
                }
                context.Services.AddAbpMiniProfiler(options =>
                {
                    options.EnableMvcFilterProfiling = false;
                    options.EnableMvcViewProfiling = false;
                });
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseMiniProfiler();
        }
    }
}