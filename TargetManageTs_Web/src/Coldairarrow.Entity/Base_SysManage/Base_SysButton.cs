using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 系统按钮
    /// </summary>
    [Table("Base_SysButton")]
    public class Base_SysButton
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        public String Icon { get; set; }

        /// <summary>
        /// ButtonCode
        /// </summary>
        public String ButtonCode { get; set; }

        /// <summary>
        /// SortNum
        /// </summary>
        public Int32? SortNum { get; set; }

        /// <summary>
        /// SysButtonTypeId
        /// </summary>
        public String SysButtonTypeId { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public String Remarks { get; set; }

        /// <summary>
        /// ButtonName
        /// </summary>
        public String ButtonName { get; set; }

        /// <summary>
        /// ButtonCodeName
        /// </summary>
        public String ButtonCodeName { get; set; }

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