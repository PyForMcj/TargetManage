using Coldairarrow.Entity;
using Coldairarrow.Entity.Base_SysManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// Jwt Token 操作功能接口
    /// </summary>
    public interface IJwtAppService
    {
        /// <summary>
        /// 新增 Jwt token
        /// </summary>
        /// <param name="dto">用户信息数据传输对象</param>
        /// <returns></returns>
        JwtAuthorizationDto Create(Base_User dto);

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="dto">用户信息数据传输对象</param>
        /// <returns></returns>
        JwtAuthorizationDto RefreshAsync(string token, Base_User dto);

        /// <summary>
        /// 判断当前 Token 是否有效
        /// </summary>
        /// <returns></returns>
        bool IsCurrentActiveTokenAsync();

        /// <summary>
        /// 停用当前 Token
        /// </summary>
        /// <returns></returns>
        void DeactivateCurrentAsync();

        /// <summary>
        /// 判断 Token 是否有效
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        bool IsActiveAsync(string token);

        /// <summary>
        /// 停用 Token
        /// </summary>
        /// <returns></returns>
        void DeactivateAsync(string token);

    }
}
