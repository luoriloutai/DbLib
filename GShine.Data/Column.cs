using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GShine.Data
{
    public class Column
    {
        /// <summary>
        /// 以列名初始化对象，其值为null
        /// </summary>
        /// <param name="name"></param>
        public Column(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 用列名和其对应的值初始化对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public Column(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        private string name = "";
        /// <summary>
        /// 列名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private object value = null;
        /// <summary>
        /// 对应列的值
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
