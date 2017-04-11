using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    /// <summary>
    /// 通配符在查询值的位置，之前、之后或者前后都有。
    /// </summary>
    public enum Wildcard
    {
        /// <summary>
        /// 前后都有通配符
        /// </summary>
        All,
        /// <summary>
        /// 只在前面有通配符
        /// </summary>
        Front,
        /// <summary>
        /// 只在后面有通配符
        /// </summary>
        Back
    }
}
