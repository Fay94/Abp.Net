using StackExchange.Profiling;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MvcExtensions
    {
        public static IMiniProfilerBuilder AddAbpMiniProfiler(this IServiceCollection services, Action<MiniProfilerOptions> configureOptions = null)
        {
            return services.AddMiniProfiler(options =>
            {
                configureOptions?.Invoke(options);
                options.RouteBasePath = "/profiler";
            })
            .AddEntityFramework();
        }
    }
}