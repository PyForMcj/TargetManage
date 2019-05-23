using System;
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
        /// IconCls
        /// </summary>
        public String IconCls { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public String Link { get; set; }

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
        /// NavTag
        /// </summary>
        public String NavTag { get; set; }

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
}