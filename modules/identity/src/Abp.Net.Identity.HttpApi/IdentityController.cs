using Abp.Net.Identity.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp.Net.Identity;

public abstract class IdentityController : AbpControllerBase
{
    protected IdentityController()
    {
        LocalizationResource = typeof(IdentityResource);
    }
}
