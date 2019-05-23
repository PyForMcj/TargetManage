using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 菜单与按钮关系表
    /// </summary>
    [Table("Base_SysNavButtonMap")]
    public class Base_SysNavButtonMap
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// BtnId
        /// </summary>
        public String BtnId { get; set; }

        /// <summary>
        /// NavId
        /// </summary>
        public String NavId { get; set; }

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