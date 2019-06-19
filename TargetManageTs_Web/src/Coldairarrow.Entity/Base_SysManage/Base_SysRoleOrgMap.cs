using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// Base_SysRoleOrgMap
    /// </summary>
    [Table("Base_SysRoleOrgMap")]
    public class Base_SysRoleOrgMap
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
        /// OrgId
        /// </summary>
        public String OrgId { get; set; }

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