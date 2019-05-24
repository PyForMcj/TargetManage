using Coldairarrow.Entity.Base_SysManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 用户操作日志记录
    /// </summary>
    public interface IUserOperationLog
    {
        /// <summary>
        /// 写入用户操作日志
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void WriteLog( string msg);
    }
}
