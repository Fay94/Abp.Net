using Abp.Net.Identity.Localization;
using Volo.Abp.Application.Services;

namespace Abp.Net.Identity;

public abstract class IdentityAppService : ApplicationService
{
    protected IdentityAppService()
    {
        LocalizationResource = typeof(IdentityResource);
        ObjectMapperContext = typeof(IdentityApplicationModule);
    }
}
