using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity.Base_SysManage
{
    /// <summary>
    /// Base_Organization
    /// </summary>
    [Table("Base_Organization")]
    public class Base_Organization
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// ParentID
        /// </summary>
        public String ParentID { get; set; }

        /// <summary>
        /// OrgName
        /// </summary>
        public String OrgName { get; set; }

        /// <summary>
        /// OrgFullName
        /// </summary>
        public String OrgFullName { get; set; }

        /// <summary>
        /// OrgCode
        /// </summary>
        public String OrgCode { get; set; }

        /// <summary>
        /// ParentOrgCode
        /// </summary>
        public String ParentOrgCode { get; set; }

        /// <summary>
        /// OrgType
        /// </summary>
        public String OrgType { get; set; }

        /// <summary>
        /// OrgLevle
        /// </summary>
        public Int32? OrgLevle { get; set; }

        /// <summary>
        /// Path
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// Creator
        /// </summary>
        public String Creator { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// ModifiedUser
        /// </summary>
        public String ModifiedUser { get; set; }

        /// <summary>
        /// ModifiedTime
        /// </summary>
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// DeleteFlag
        /// </summary>
        public Boolean? DeleteFlag { get; set; }

        /// <summary>
        /// Order
        /// </summary>
        public Int32? Order { get; set; }

    }
}