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
        public void WriteLog(string msg)
        {
            string userName = null, ipAddress = HttpContextCore.Current.Connection.RemoteIpAddress.ToString();
            try
            {
                userName = Base_UserBusiness.GetCurrentUser().UserName;
            }
            catch
            {

            }
            Base_SysUserOperationLog sysUserOperationLog = new Base_SysUserOperationLog
            {
                Id = GuidHelper.GenerateKey(),
                LogType = EnumType.LogType.用户操作.ToString(),
                LogContent = $"[{ipAddress}]地址的[{userName}]用户操作记录：{msg}",
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
