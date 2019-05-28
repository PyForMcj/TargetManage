using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// ϵͳ���û���
    /// </summary>
    [Table("Base_User")]
    [Serializable]
    public class Base_User
    {

        /// <summary>
        /// ��������
        /// </summary>
        [Key]
        public String Id { get; set; }

        /// <summary>
        /// �û�Id,�߼�����
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// �û���
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// ��ʵ����
        /// </summary>
        public String RealName { get; set; }

        /// <summary>
        /// �Ա�(1Ϊ�У�0ΪŮ)
        /// </summary>
        public Int32? Sex { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// �ʼ�
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// �绰
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// �û����ɫ��ϵ����
        /// </summary>
        [NotMapped]
        public IQueryable<Base_UserRoleMap> Base_UserRoleMaps { get; set; }

        /// <summary>
        /// ��ɫ����
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysRole> Base_SysRoles { get; set; }

        /// <summary>
        /// �˵�����
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysNavigation> Base_SysNavigations{ get; set; }

        /// <summary>
        /// ��ť����
        /// </summary>
        [NotMapped]
        public IQueryable<Base_SysButton> Base_SysButtons { get; set; }
    }
    
}