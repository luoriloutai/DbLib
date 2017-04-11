using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class SelectCommand : Command
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SelectCommand(string connectionString)
            : base(connectionString)
        {

        }

        /// <summary>
        /// 查询单个表指定列的全部数据。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnNames">查询列数组</param>
        /// <returns></returns>
        public DataTable SelectDataTable(string tableName, string[] columnNames)
        {
            string columnString = GetSelectColumnString(columnNames);
            string sql = string.Format("select {0} from {1} where 1=1 ", columnString, tableName);
            DB db = new DB(this.ConnectionString);
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 查询单个表指定列的全部数据。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnNames">查询列数组</param>
        /// <returns></returns>
        public object[][] SelectArray(string tableName, string[] columnNames)
        {
            string columnString = GetSelectColumnString(columnNames);
            string sql = string.Format("select {0} from {1} where 1=1 ", columnString, tableName);
            DB db = new DB(this.ConnectionString);
            object[][] rows = db.GetDataTableArray(sql);
            return rows;
        }

        /// <summary>
        /// 注意，该方法不可首先调用，在调用该方法之前调用Join系列、Where系列方法。查询所有列。
        /// </summary>
        /// <returns></returns>
        public DataTable SelectDataTable()
        {
            string sql = "select * from " + queryTable + "where 1=1" + WhereString;
            DB db = new DB(this.ConnectionString);
            SQLCommand.CommandText = sql;
            return db.GetDataTable(SQLCommand);
        }

        /// <summary>
        /// 注意，该方法不可首先调用，在调用该方法之前调用Join系列、Where系列方法。查询所有列。
        /// </summary>
        /// <returns></returns>
        public object[][] SelectArray()
        {
            string sql = "select * from" + queryTable + "where 1=1" + WhereString;
            DB db = new DB(this.ConnectionString);
            SQLCommand.CommandText = sql;
            object[][] rows = db.GetDataTableArray(SQLCommand);
            return rows;
        }

        /// <summary>
        /// 获取指定表的所有数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public DataTable SelectDataTable(string tableName)
        {
            string sql = string.Format("select * from {0}", tableName);
            DB db = new DB(this.ConnectionString);
            return db.GetDataTable(sql);
        }

        /// <summary>
        /// 获取指定表的所有数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public object[][] SelectArray(string tableName)
        {
            string sql = string.Format("select * from {0}", tableName);
            DB db = new DB(this.ConnectionString);
            object[][] rows = db.GetDataTableArray(sql);
            return rows;
        }

        /// <summary>
        /// 带参数的查询
        /// </summary>
        /// <param name="sql">查询语句，其参数格式为：@0，@1……</param>
        /// <param name="values">对应值数组</param>
        /// <returns></returns>
        public DataTable SelectDataTable(string sql, params object[] values)
        {
            DB db = new DB(this.ConnectionString);
            return db.GetDataTable(sql, values);
        }

        /// <summary>
        /// 带参数的查询
        /// </summary>
        /// <param name="sql">查询语句，其参数格式为：@0，@1……</param>
        /// <param name="values">对应值数组</param>
        /// <returns></returns>
        public object[][] SelectArray(string sql, params object[] values)
        {
            DB db = new DB(this.ConnectionString);
            object[][] rows = db.GetDataTableArray(sql, values);
            return rows;
        }

        private string queryTable = "";

        /// <summary>
        /// 构建查询表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public SelectCommand Table(string tableName)
        {
            queryTable += " " + tableName + " ";
            return this;
        }

        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public SelectCommand InnerJoin(string tableName, string on)
        {
            queryTable += " inner join " + tableName + " on " + on + " ";
            return this;
        }

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public SelectCommand LeftJoin(string tableName, string on)
        {
            queryTable += " left join " + tableName + " on " + on + " ";
            return this;
        }

        /// <summary>
        /// 右连接
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public SelectCommand RightJoin(string tableName, string on)
        {
            queryTable += " right join " + tableName + " on " + on + " ";
            return this;
        }

        /// <summary>
        /// 只有一个分组列的分组。注意，查询列如果不是包含在聚合函数中，那么调用只有一个分组列的方法可能出错，此时应调用有多列分组的重载方法。
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="having"></param>
        /// <returns></returns>
        public SelectCommand Group(string columnName, string having)
        {
            queryTable += " group by " + columnName + " having " + having + " ";
            return this;
        }

        /// <summary>
        /// 分组查询
        /// </summary>
        /// <param name="columnNames">要分组的列名数组</param>
        /// <param name="having">分组条件</param>
        /// <returns></returns>
        public SelectCommand Group(string[] columnNames, string having)
        {
            queryTable += " group by ";
            for (int i = 0; i < columnNames.Length; i++)
            {
                queryTable += columnNames[i] + ",";
            }

            queryTable = queryTable.Remove(queryTable.Length - 1, 1);
            queryTable += " " + having + " ";
            return this;
        }

        private string GetOrderTypeString(OrderType type)
        {
            string typeString = "";
            switch (type)
            {
                case OrderType.ASC:
                    typeString = " asc ";
                    break;
                case OrderType.DESC:
                    typeString = " desc ";
                    break;
            }
            return typeString;
        }

        /// <summary>
        /// 以单个字段排序
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="type">排序类型</param>
        /// <returns></returns>
        public SelectCommand Order(string columnName, OrderType type)
        {
            queryTable += " order by " + columnName + GetOrderTypeString(type);
            return this;
        }

        /// <summary>
        /// 以多个字段排序
        /// </summary>
        /// <param name="columnNames">列名数组</param>
        /// <param name="type">排序类型</param>
        /// <returns></returns>
        public SelectCommand Order(string[] columnNames, OrderType type)
        {
            queryTable += " order by ";
            for (int i = 0; i < columnNames.Length; i++)
            {
                queryTable += columnNames[i] + ",";
            }
            queryTable = queryTable.Remove(queryTable.Length - 1, 1);
            queryTable += " " + GetOrderTypeString(type);
            return this;
        }



        /// <summary>
        /// 联合，去重。之后调用join系列方法。
        /// </summary>
        /// <param name="tableName">联合的第一个表名</param>
        /// <returns></returns>
        public SelectCommand Union(string tableName)
        {
            queryTable += " union select " + tableName + " ";
            return this;
        }

        /// <summary>
        /// 联合，不去重。之后调用join系列方法。
        /// </summary>
        /// <param name="tableName">联合的第一个表名</param>
        /// <returns></returns>
        public SelectCommand UnionAll(string tableName)
        {
            queryTable += " union all select " + tableName + " ";
            return this;
        }


        private string GetSelectColumnString(string tableName, string[] columnNames)
        {
            string columnString = "";
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnString += tableName + "." + columnNames[i] + ",";
            }

            if (columnNames.Length > 0)
            {
                columnString = columnString.Remove(columnString.Length - 1, 1);
            }

            if (columnString == "")
            {
                return tableName + ".*";
            }
            return columnString;
        }

        private string GetSelectColumnString(string[] columnNames)
        {
            string columnString = "";
            for (int i = 0; i < columnNames.Length; i++)
            {
                columnString += columnNames[i] + ",";
            }

            if (columnNames.Length > 0)
            {
                columnString = columnString.Remove(columnString.Length - 1, 1);
            }

            if (columnString == "")
            {
                return "*";
            }
            return columnString;
        }

        /// <summary>
        /// 指定Where条件，该方法可以连续调用。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <param name="value">列对应的值</param>
        /// <returns></returns>
        public new SelectCommand Where(LogicOperator logicOperator, string columnName, ValueOperator valueOperator, object value)
        {
            base.Where(logicOperator, columnName, valueOperator, value);
            return this;
        }

        /// <summary>
        ///指定Like条件，该方法可以连续调用
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="value">列对应的值</param>
        /// <param name="position">通配符位置</param>
        /// <returns></returns>
        public new SelectCommand LikeWhere(LogicOperator logicOperator, string columnName, object value, Wildcard position)
        {
            base.LikeWhere(logicOperator, columnName, value, position);
            return this;
        }

        /// <summary>
        /// 指定某列值为NULL。该方法可连续调用。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <returns></returns>
        public new SelectCommand NullWhere(LogicOperator logicOperator, string columnName, NullValueOperator valueOperator)
        {
            base.NullWhere(logicOperator, columnName, valueOperator);
            return this;
        }


        /// <summary>
        /// 分组左包裹，分组查询左半个括号。该方法与GroupWrapRight()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <returns></returns>
        public new SelectCommand GroupWrapLeft(LogicOperator logicOperator)
        {
            base.GroupWrapLeft(logicOperator);
            return this;
        }

        /// <summary>
        /// 分组右包裹，分组查询右半个括号。该方法与GroupWrapLeft()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <returns></returns>
        public new SelectCommand GroupWrapRight()
        {
            base.GroupWrapRight();
            return this;
        }

    }
}
