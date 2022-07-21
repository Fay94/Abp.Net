using Abp.Net.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp.Net.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NetController : AbpControllerBase
{
    protected NetController()
    {
        LocalizationResource = typeof(NetResource);
    }
}
