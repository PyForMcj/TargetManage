using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 针对中间件的扩展类
    /// </summary>
    public static class IApplicationBuilderExtension
    {
        /// <summary>
        /// 参数映射扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        [Obsolete]
        public static IApplicationBuilder UseParameterMappingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<String>();
        }
    }
}
