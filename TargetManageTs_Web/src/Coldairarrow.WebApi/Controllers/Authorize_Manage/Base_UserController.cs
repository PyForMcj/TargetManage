﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Coldairarrow.Entity;
using Coldairarrow.Business.Base_SysManage;
using Coldairarrow.Util;
using Coldairarrow.Entity.Base_SysManage;

namespace Coldairarrow.WebApi.Controllers.Authorize_Manage
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [ApiExplorerSettings(GroupName = "权限API")]
    public class Base_UserController : ControllerBase
    {
        Base_UserBusiness base_UserBusiness = new Base_UserBusiness();

        #region 视图功能

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetData")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "查询用户" })]
        public IActionResult GetData(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_User() : base_UserBusiness.GetTheData(id);

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "",
                Data = theData,
                ErrorCode = 0
            });
        }

        #endregion

        #region 获取数据

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagination">分页</param>
        /// <returns></returns>
        [HttpPost("GetDataList")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "查询用户" })]
        public IActionResult GetDataList(string condition, string keyword, Pagination pagination)
        {
            var dataList = base_UserBusiness.GetDataList(condition, keyword, pagination);

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "",
                Data = pagination.BuildTableResult_DataGrid(dataList),
                ErrorCode = 0
            });
        }

        #endregion
        
        #region 提交数据

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [HttpPost("SaveData")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "新增或修改用户" })]
        public ActionResult SaveData(Base_User theData)
        {
            string msg = string.Empty;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                base_UserBusiness.AddData(theData);
                msg = "添加成功！";
            }
            else
            {
                base_UserBusiness.UpdateData(theData);
                msg = "修改成功！";
            }

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = msg,
                Data = null,
                ErrorCode = 0
            });
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">删除的数据id</param>
        [HttpGet("DeleteData")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "删除用户" })]
        public ActionResult DeleteData(string ids)
        {
            base_UserBusiness.DeleteData(ids.ToList<string>());

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "删除成功！",
                Data = null,
                ErrorCode = 0
            });
        }

        #endregion

        #region 根据Token获取当前登录用户的权限等所有信息
        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("GetCurrentUserInfo")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "查询用户" })]
        public IActionResult GetCurrentUserInfo(string token)
        {
            var userInfoDic = JWTHelper.GetPayLoad(token);
            var userId = userInfoDic["UserId"]?.ToString();
            var data = userId.IsNullOrEmpty() ? new Base_User() : base_UserBusiness.GetCurrentUserInfo(userId);
            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "",
                Data = data,
                ErrorCode = 0
            });
        }
        #endregion
    }

}