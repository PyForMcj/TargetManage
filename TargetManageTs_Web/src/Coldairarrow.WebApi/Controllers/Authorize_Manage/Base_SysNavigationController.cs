using System;
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
    /// 菜单管理控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Permission")]
    [ApiExplorerSettings(GroupName = "权限API")]
    public class Base_SysNavigationController : ControllerBase
    {
        Base_SysNavigationBusiness _base_SysNavigationBusiness = new Base_SysNavigationBusiness();

        #region 视图功能

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetData")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "查询菜单" })]
        public IActionResult GetData(string id)
        {
            var theData = id.IsNullOrEmpty() ? new Base_SysNavigation() : _base_SysNavigationBusiness.GetTheData(id);

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
        /// <param name="id">id为空获取所有的数据，不为空获取所有parentId等于它的数据</param>
        /// <returns></returns>
        [HttpPost("GetDataList")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "查询菜单" })]
        public IActionResult GetDataList(string condition, string keyword, Pagination pagination, string id = "")
        {
            var dataList = _base_SysNavigationBusiness.GetDataList(condition, keyword, pagination, id);

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "",
                Data = pagination.BuildTableResult_DataGrid(dataList).ToJson(),
                ErrorCode = 0
            });
        }

        #endregion

        #region 获取菜单树
        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenuTrees")]
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "获取菜单树" })]
        public IActionResult GetMenuTrees()
        {
            var dataList = _base_SysNavigationBusiness.GetMenuTrees();

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "",
                Data = dataList,
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
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "新增或修改菜单" })]
        public ActionResult SaveData(Base_SysNavigation theData)
        {
            string msg = string.Empty;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = Guid.NewGuid().ToSequentialGuid();

                _base_SysNavigationBusiness.AddData(theData);
                msg = "添加成功！";
            }
            else
            {
                _base_SysNavigationBusiness.UpdateData(theData);
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
        [TypeFilter(typeof(UserOperationLogAttribute), Arguments = new object[] { "删除菜单" })]
        public ActionResult DeleteData(string ids)
        {
            _base_SysNavigationBusiness.DeleteData(ids.ToList<string>());

            return Ok(new AjaxResult
            {
                Success = true,
                Msg = "删除成功！",
                Data = null,
                ErrorCode = 0
            });
        }

        #endregion
    }

}