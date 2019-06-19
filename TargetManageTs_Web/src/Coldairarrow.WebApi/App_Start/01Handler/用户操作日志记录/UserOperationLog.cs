using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coldairarrow.Business;
using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 用户操作日志
    /// </summary>
    public class UserOperationLog : BaseBusiness<Base_SysUserOperationLog>, IUserOperationLog
    {
        /// <summary>
        /// 登录之后都采用此特性记录日志
        /// 登录之后请求都传递Token
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLog(string msg)
        {
            //获取Ip地址的临时写法，这个写法不准确，没考虑代理
            string userName = null, ipAddress = HttpContextCore.Current.Connection.RemoteIpAddress.ToString();
            try
            {
                //userName = Base_UserBusiness.GetCurrentUser().UserName;
                userName = CurrentUserHelper.GetCurrentUserInfo()?.UserName;
            }
            catch
            {

            }
            Base_SysUserOperationLog sysUserOperationLog = new Base_SysUserOperationLog
            {
                Id = GuidHelper.GenerateKey(),
                LogType = EnumType.LogType.用户操作.ToString(),
                LogContent = $"[{DateTime.Now.ToCstTime().ToString("yyyy-MM-dd HH:mm:ss")}][{ipAddress}]地址的[{userName}]用户操作记录：{msg}",
                OpTime = DateTime.Now.ToCstTime(),
                OpUserName = userName
            };
            Task.Run(() =>
            {
                try
                {
                    Insert(sysUserOperationLog);
                }
                catch
                {

                }
            });
        }
    }
}
