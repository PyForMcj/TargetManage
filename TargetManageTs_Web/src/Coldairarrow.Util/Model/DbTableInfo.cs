﻿namespace Coldairarrow.Util
{
    /// <summary>
    /// 数据库所有表的信息
    /// </summary>
    public class DbTableInfo
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述说明
        /// </summary>
        public string Description
        {
            get
            {
                return _description.IsNullOrEmpty() ? TableName : _description;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// 是否已创建
        /// </summary>
        public string IsCreate { get; set; } = "否";

        private string _description { get; set; }
    }
}
