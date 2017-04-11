using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    /// <summary>
    /// 值操作符枚举
    /// </summary>
    public enum ValueOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        Equal,
        /// <summary>
        /// 大于
        /// </summary>
        Greater,
        /// <summary>
        /// 小于
        /// </summary>
        Less,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqual,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqual,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual
    }
}
