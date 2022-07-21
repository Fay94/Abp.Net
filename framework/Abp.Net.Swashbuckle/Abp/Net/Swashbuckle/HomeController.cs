using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Abp.Net.Swashbuckle
{
    public class HomeController : AbpController
    {
        protected SwashbuckleOptions Options { get; }
        public HomeController(SwashbuckleOptions options)
        {
            Options = options;
        }
        [HttpPost, AllowAnonymous]
        public int CheckUrl()
        {
            return 401;
        }
        [HttpPost, AllowAnonymous]
        public int SubmitUrl([FromForm] SpecificationAuth auth)
        {
            if (auth.UserName == Options.UserName && auth.Password == Options.Password)
            {
                return 200;
            }
            else
            {
                return 401;
            }
        }
    }
}
