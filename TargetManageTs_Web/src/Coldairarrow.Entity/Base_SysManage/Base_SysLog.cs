using Nest;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// ϵͳ��־��
    /// </summary>
    [Table("Base_SysLog")]
    public class Base_SysLog
    {

        /// <summary>
        /// ��������
        /// </summary>
        [Key]
        [Keyword]
        public String Id { get; set; }

        /// <summary>
        /// ��־����
        /// </summary>
        [Keyword]
        public String LogType { get; set; }

        /// <summary>
        /// ��־����
        /// </summary>
        [Keyword]
        public String LogContent { get; set; }

        /// <summary>
        /// ����Ա�û���
        /// </summary>
        [Keyword]
        public String OpUserName { get; set; }

        /// <summary>
        /// ��־��¼ʱ��
        /// </summary>
        [Keyword]
        public DateTime? OpTime { get; set; }

    }
}