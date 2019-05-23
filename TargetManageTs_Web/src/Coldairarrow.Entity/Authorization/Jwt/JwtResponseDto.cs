using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// Jwt 响应对象
    /// </summary>
    public class JwtResponseDto
    {
        /// <summary>
        /// 访问 Token 值
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string Type { get; set; } = "Bearer";

        /// <summary>
        /// 个人信息
        /// </summary>
        public Profile Profile { get; set; }
    }

    /// <summary>
    /// 个人信息
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// 用户Id,逻辑主键
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 授权时间戳
        /// </summary>
        public long Auths { get; set; }

        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long Expires { get; set; }
    }
}
