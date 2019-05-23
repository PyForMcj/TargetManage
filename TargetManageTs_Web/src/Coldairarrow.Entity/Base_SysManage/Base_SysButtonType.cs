using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 系统按钮类型
    /// </summary>
    [Table("Base_SysButtonType")]
    public class Base_SysButtonType
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// TypeName
        /// </summary>
        public String TypeName { get; set; }

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
}