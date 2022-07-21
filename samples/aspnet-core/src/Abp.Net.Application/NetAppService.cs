using System;
using System.Collections.Generic;
using System.Text;
using Abp.Net.Localization;
using Volo.Abp.Application.Services;

namespace Abp.Net;

/* Inherit your application services from this class.
 */
public abstract class NetAppService : ApplicationService
{
    protected NetAppService()
    {
        LocalizationResource = typeof(NetResource);
    }
}
