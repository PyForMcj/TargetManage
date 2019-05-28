using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// 系统，用户表
    /// </summary>
    [Table("Base_User")]
    [Serializable]
    public class Base_User
    {

        /// <summary>
        /// 代理主键
        /// </summary>
        [Key]
        public String Id { get; set; }

        /// <summary>
        /// 用户Id,逻辑主键
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// 性别(1为男，0为女)
        /// </summary>
        public Int32? Sex { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 用户与角色关系集合
        /// </summary>
        [NotMapped]
        public IQueryable<Base_UserRoleMap> Base_UserRoleMaps { get; set; }

        /// <summary>
        /// 角色集合
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysRole> Base_SysRoles { get; set; }

        /// <summary>
        /// 菜单集合
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysNavigation> Base_SysNavigations{ get; set; }

        /// <summary>
        /// 按钮集合
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysButton> Base_SysButtons { get; set; }
    }
    
}