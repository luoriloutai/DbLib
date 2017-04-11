using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    /// <summary>
    /// 更新命令
    /// </summary>
    public class UpdateCommand : Command
    {
        private Record record;

        /// <summary>
        /// 获取或设置记录
        /// </summary>
        public Record Record
        {
            get { return record; }
            set { record = value; }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        public UpdateCommand(string tableName, string connectionString)
            : base(tableName, connectionString)
        {
            Table table = new Table(tableName, connectionString);
            string[] columnNames = table.GetColumnNames();
            record = new Record();
            //  初始化记录
            for (int i = 0; i < columnNames.Length; i++)
            {
                record.Columns.Add(new Column(columnNames[i]));
            }
        }

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="columnNames">更新的列名数组</param>
        public UpdateCommand(string tableName, string connectionString, params string[] columnNames)
            : base(tableName, connectionString)
        {
            record = new Record();
            //  初始化记录
            for (int i = 0; i < columnNames.Length; i++)
            {
                record.Columns.Add(new Column(columnNames[i]));
            }
        }


        /// <summary>
        /// 注意，在调用该方法之前注意先调用Where或LikeWhere方法以更新需要更新的数据，不可首先调用。
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            int effect = 0;
            string sql = "update " + TableName + " set ";
            string id = "";
            string parameterName = "";
            for (int i = 0; i < record.Columns.Count; i++)
            {
                if (record.Columns[i].Value != null)
                {
                    id = DateTime.Now.ToFileTime().ToString();
                    parameterName = "@" + record.Columns[i].Name + "_" + id;
                    sql += record.Columns[i].Name + " = " + parameterName + ",";
                    SQLCommand.Parameters.AddWithValue(parameterName, record.Columns[i].Value);
                }
            }

            sql = sql.Remove(sql.Length - 1, 1);
            sql += " where 1=1 ";
            SQLCommand.CommandText = sql + WhereString;
            SQLCommand.Connection.Open();
            effect = SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effect;
        }

        /// <summary>
        /// 指用指定条件更新数据，该方法不需调用Where、LikeWhere等方法,也不需要单列指定更新的值，过滤条件由where指定。
        /// </summary>
        /// <param name="columnNames">列名数组</param>
        /// <param name="values">列对应的值数组</param>
        /// <param name="where">过滤条件,最前边带连接关键字and、or</param>
        /// <returns></returns>
        public int Update(string[] columnNames, object[] values, string where)
        {
            int effect = 0;
            string sql = "update " + TableName + " set ";
            string id = "";
            string parameterName = "";
            for (int i = 0; i < columnNames.Length; i++)
            {
                id = DateTime.Now.ToFileTime().ToString();
                parameterName = "@" + columnNames[i] + "_" + id;
                sql += columnNames[i] + " = " + parameterName + ",";
                SQLCommand.Parameters.AddWithValue(parameterName, values[i]);
            }

            sql = sql.Remove(sql.Length - 1, 1);
            sql += " where 1=1 ";
            SQLCommand.CommandText = sql + where;
            SQLCommand.Connection.Open();
            effect = SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effect;
        }

        /// <summary>
        /// 指用指定条件更新数据，该方法不需调用Where、LikeWhere等方法,也不需要单列指定更新的值，过滤条件由where指定，该Where带参数。
        /// </summary>
        /// <param name="columnNames">列名数组</param>
        /// <param name="values">列对应的值数组</param>
        /// <param name="where">过滤条件,最前边带连接关键字and、or，参数形式为：@0、@1</param>
        /// <param name="whereValues">where语句中参数的值，要与where中出现的位置对应</param>
        /// <returns></returns>
        public int Update(string[] columnNames, object[] columnValues, string where, object[] whereValues)
        {
            int effect = 0;
            string sql = "update " + TableName + " set ";
            string id = "";
            string parameterName = "";
            for (int i = 0; i < columnNames.Length; i++)
            {
                id = DateTime.Now.ToFileTime().ToString();
                parameterName = "@" + columnNames[i] + "_" + id;
                sql += columnNames[i] + " = " + parameterName + ",";
                SQLCommand.Parameters.AddWithValue(parameterName, columnValues[i]);
            }

            sql = sql.Remove(sql.Length - 1, 1);
            sql += " where 1=1 ";
            SQLCommand.CommandText = sql + where;
            for (int i = 0; i < whereValues.Length; i++)
            {
                SQLCommand.Parameters.AddWithValue("@" + i.ToString(), whereValues[i]);
            }

            SQLCommand.Connection.Open();
            effect = SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effect;
        }

        /// <summary>
        /// 指定Where条件，该方法可以连续调用。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <param name="value">列对应的值</param>
        /// <returns></returns>
        public new UpdateCommand Where(LogicOperator logicOperator, string columnName, ValueOperator valueOperator, object value)
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
        public new UpdateCommand LikeWhere(LogicOperator logicOperator, string columnName, object value, Wildcard position)
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
        public new UpdateCommand NullWhere(LogicOperator logicOperator, string columnName, NullValueOperator valueOperator)
        {
            base.NullWhere(logicOperator, columnName, valueOperator);
            return this;
        }

        /// <summary>
        /// 分组左包裹，分组查询左半个括号。该方法与GroupWrapRight()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <returns></returns>
        public new UpdateCommand GroupWrapLeft(LogicOperator logicOperator)
        {
            base.GroupWrapLeft(logicOperator);
            return this;
        }

        /// <summary>
        /// 分组右包裹，分组查询右半个括号。该方法与GroupWrapLeft()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <returns></returns>
        public new UpdateCommand GroupWrapRight()
        {
            base.GroupWrapRight();
            return this;
        }

    }
}
