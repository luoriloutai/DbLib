using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    /// <summary>
    /// 数据记录，包含多个列
    /// </summary>
    public class Record
    {
        private List<Column> columns = new List<Column>();

        public List<Column> Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        /// <summary>
        /// 通过列名获取列所在索引。返回-1表示未找到。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetColumnIndex(string name)
        {
            int index = -1;
            for (int i = 0; i < columns.Count; i++)
            {
                if (string.Compare(columns[i].Name, name, true) == 0)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        /// <summary>
        /// 通过指定列名获取对应列对象。未找到则返回null。
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public Column this[string columnName]
        {
            get
            {
                int index = GetColumnIndex(columnName);
                if (index != -1)
                {
                    return columns[index];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 该索引器适用于已知相应列的索引时，若未知相应列的索引，请使用参数为string类型的重载。
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public Column this[int index]
        {
            get
            {
                return columns[index];
            }
        }




    }
}
