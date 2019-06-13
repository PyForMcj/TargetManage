using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 这里暂时不存储在缓存中
    /// 在Chrome高版本中，发现Cookie无法被携带问题
    /// 导致无法定义缓存Key
    /// </summary>
    public class CurrentUserHelper
    {
        public static Base_User GetCurrentUserInfo()
        {
            var authorizationHeader = HttpContextCore.Current.Request.Headers["authorization"];
            var data = authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.ToString();
            if (data.IsNullOrEmpty())
                return null;
            var userInfoDic = JWTHelper.GetPayLoad(data);
            Base_User base_User = new Base_User
            {
                UserId = userInfoDic["UserId"]?.ToString(),
                UserName= userInfoDic["unique_name"]?.ToString(),
            };
            return base_User;
        }
    }
}
