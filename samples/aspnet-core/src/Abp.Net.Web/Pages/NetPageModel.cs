using Abp.Net.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Abp.Net.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class NetPageModel : AbpPageModel
{
    protected NetPageModel()
    {
        LocalizationResourceType = typeof(NetResource);
    }
}
