using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.AspNetCore.Builder;

public static class AbpSwaggerUIOptionsExtensions
{
    public static void EnableSwaggerExport(
        this SwaggerUIOptions options)
    {
        options.InjectStylesheet("ui/swaggerdoc.css");
        options.InjectJavascript("ui/jquery.js");
        options.InjectJavascript("ui/swaggerdoc.js");
    }
}