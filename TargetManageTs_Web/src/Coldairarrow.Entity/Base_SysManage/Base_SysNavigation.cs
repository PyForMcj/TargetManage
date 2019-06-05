using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("Base_SysNavigation")]
    public class Base_SysNavigation
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public String ParentId { get; set; }

        /// <summary>
        /// NavName
        /// </summary>
        public String NavName { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        public String Icon { get; set; }

        /// <summary>
        /// IconSvg
        /// </summary>
        public String IconSvg { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// IsShow
        /// </summary>
        public Int32? IsShow { get; set; }

        /// <summary>
        /// SortNum
        /// </summary>
        public Int32? SortNum { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CraeteUserId
        /// </summary>
        public String CraeteUserId { get; set; }

        /// <summary>
        /// CraeteUserName
        /// </summary>
        public String CraeteUserName { get; set; }

    }

    /// <summary>
    /// 前段菜单树用
    /// </summary>
    public class Base_SysNavigationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 路由 path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// SVG 菜单图标
        /// </summary>
        public string IconSvg { get; set; }

        /// <summary>
        /// Tree树名字
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 子菜单数据
        /// </summary>
        public List<Base_SysNavigationDto> Children { get; set; }
    }
}