﻿using System.Reflection;
using System.Text;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Swashbuckle;

namespace Abp.Net.MiniProfiler
{
    public class MiniProfilerHtmlResolver : ISwaggerHtmlResolver, ITransientDependency// SwaggerHtmlResolver
    {
        public virtual Stream Resolver()
        {
            var thisType = GetType();
            var stream = thisType.GetTypeInfo().Assembly.GetManifestResourceStream($"{thisType.Namespace}.index.html");

            var html = new StreamReader(stream)
                .ReadToEnd()
                .Replace("SwaggerUIBundle(configObject)", "abp.SwaggerUIBundle(configObject)");

            return new MemoryStream(Encoding.UTF8.GetBytes(html));
        }
    }
}