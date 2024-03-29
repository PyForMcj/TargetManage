﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class SecretDto
    {
        /// <summary>
        /// 账号名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 登录后授权的 Token
        /// 刷新 Toekn 的时候使用
        /// </summary>
        public string Token { get; set; }
    }
}
