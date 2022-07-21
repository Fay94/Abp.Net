using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Abp.Net;

[Dependency(ReplaceServices = true)]
public class NetBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Net";
}
