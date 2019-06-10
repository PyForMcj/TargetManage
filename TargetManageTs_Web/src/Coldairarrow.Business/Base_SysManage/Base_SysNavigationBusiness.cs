using Coldairarrow.Entity.Base_SysManage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Coldairarrow.Business.Base_SysManage
{
    public class Base_SysNavigationBusiness : BaseBusiness<Base_SysNavigation>
    {
        private const string topNavId = "00000-00000-00000-00000-00000";

        #region 外部接口

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="condition">查询类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="id">id为空获取所有的数据，不为空获取所有parentId等于它的数据</param>
        /// <returns></returns>
        public List<Base_SysNavigation> GetDataList(string condition, string keyword, Pagination pagination, string id = "")
        {
            var q = GetIQueryable();

            //模糊查询
            if (!condition.IsNullOrEmpty() && !keyword.IsNullOrEmpty())
                q = q.Where($@"{condition}.Contains(@0)", keyword);
            if (!id.IsNullOrEmpty() && !id.Equals(topNavId))
                q = q.Where(c => c.ParentId.Equals(id));
            if (!id.IsNullOrEmpty() && id.Equals(topNavId))
                q = q.Where(c => c.ParentId.IsNullOrEmpty());
            return q.GetPagination(pagination).ToList();
        }

        /// <summary>
        /// 获取所有数据列表
        /// </summary>
        /// <returns></returns>
        public List<Base_SysNavigationDto> GetMenuTrees()
        {
            var q= GetIQueryable().ToList();
            var topNav = q.Where(c => string.IsNullOrEmpty(c.ParentId)).ToList();
            var treeNav = Base_UserBusiness.navigationDtos(q, topNav);
            Base_SysNavigationDto sysnav = new Base_SysNavigationDto
            {
                Id = topNavId,
                Label = "顶级菜单",
                Children= treeNav
            };
            var listData = new List<Base_SysNavigationDto>();
            listData.Add(sysnav);
            return listData;
        }

        /// <summary>
        /// 获取指定的单条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public Base_SysNavigation GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newData">数据</param>
        public void AddData(Base_SysNavigation newData)
        {
            Insert(newData);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        public void UpdateData(Base_SysNavigation theData)
        {
            Update(theData);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="theData">删除的数据</param>
        public void DeleteData(List<string> ids)
        {
            Delete(ids);
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}