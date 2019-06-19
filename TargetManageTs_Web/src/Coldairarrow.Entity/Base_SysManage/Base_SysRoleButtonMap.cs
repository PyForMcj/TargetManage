using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 角色与按钮关联表
    /// </summary>
    [Table("Base_SysRoleButtonMap")]
    public class Base_SysRoleButtonMap
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public String RoleId { get; set; }

        /// <summary>
        /// BtnId
        /// </summary>
        public String BtnId { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// CreateUserId
        /// </summary>
        public String CreateUserId { get; set; }

        /// <summary>
        /// CreateUserName
        /// </summary>
        public String CreateUserName { get; set; }

    }
}