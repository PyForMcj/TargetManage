using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 用户操作记录过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public class UserOperationLogAttribute : Attribute,IActionFilter
    {
        /// <summary>
        /// 记录消息
        /// </summary>
        private readonly string _msg;

        /// <summary>
        /// 用户操作日志
        /// </summary>
        private readonly IUserOperationLog _userOperationLog;

        public UserOperationLogAttribute(string msg,IUserOperationLog userOperationLog)
        {
            _msg = msg;
            _userOperationLog = userOperationLog;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //暂定只有成功执行方法的才记录日志
            //出错误的话直接去错误日志去找记录去吧
            if (context.HttpContext.Response.StatusCode==StatusCodes.Status200OK)
            {
                _userOperationLog.WriteLog(_msg);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
