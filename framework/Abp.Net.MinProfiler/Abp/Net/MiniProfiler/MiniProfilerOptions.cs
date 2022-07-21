namespace Abp.Net.MiniProfiler
{
    public class MiniProfilerOptions
    {
        public bool IsEnabled { get; set; }

        public bool InjectSwagger { get; set; }

        public MiniProfilerOptions()
        {
            IsEnabled = true;
            InjectSwagger = true;
        }
    }
}
