using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 用户操作日志表
    /// </summary>
    [Table("Base_SysUserOperationLog")]
    public class Base_SysUserOperationLog
    {

        /// <summary>
        /// 代理主键
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 日志类别
        /// </summary>
        public String LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public String LogContent { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public String OpUserName { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OpTime { get; set; }

    }
}