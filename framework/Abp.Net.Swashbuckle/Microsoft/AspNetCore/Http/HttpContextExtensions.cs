using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// 设置Swagger自动登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="accessToken"></param>
        public static void SigninToSwagger(this HttpContext httpContext, string accessToken)
        {
            // 设置 Swagger 刷新自动授权
            httpContext.Response.Headers["access-token"] = accessToken;
        }

        /// <summary>
        /// 设置Swagger退出登录
        /// </summary>
        /// <param name="httpContext"></param>
        public static void SignoutToSwagger(this HttpContext httpContext)
        {
            httpContext.Response.Headers["access-token"] = "invalid_token";
        }
    }
}
