using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.WebApi
{
    /// <summary>
    /// 全局参数序列化
    /// 针对Webapi实体参数问题处理
    /// </summary>
    public class GlobalActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //参数映射：支持application/json
            string contentType = context.HttpContext.Request.ContentType;
            if (!contentType.IsNullOrEmpty() && contentType.Contains("application/json"))
            {
                var actionParameters = context.ActionDescriptor.Parameters;
                var allParamters = HttpHelper.GetAllRequestParams(context.HttpContext);
                var actionArguments = context.ActionArguments;
                actionParameters.ForEach(aParamter =>
                {
                    string key = aParamter.Name;
                    if (allParamters.ContainsKey(key))
                    {
                        //这个地方是为了分页单独处理，只针对分页返回数据的
                        //其他的数据前台返回具体的实体字段
                        //因为分页返回数据包含有其他关键字查询等，没有办法全部序列化
                        if (aParamter.ParameterType.Name.Equals("Pagination"))
                        {
                            try
                            {
                                actionArguments[key] = allParamters[key].ToJson().ToObject(aParamter.ParameterType);
                            }
                            catch
                            {

                            }
                        }
                        else
                            actionArguments[key] = allParamters[key]?.ToString()?.ChangeType(aParamter.ParameterType);
                    }
                    else
                    {
                        try
                        {
                            actionArguments[key] = allParamters.ToJson().ToObject(aParamter.ParameterType);
                        }
                        catch
                        {

                        }
                    }
                });
            }
        }
    }
}
