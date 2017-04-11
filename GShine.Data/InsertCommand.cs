using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GShine.Data
{
    /// <summary>
    /// 写入数据命令
    /// </summary>
    public class InsertCommand
    {
        private Record record;

        /// <summary>
        /// 表示一条数据，由多列组成。
        /// </summary>
        public Record Record
        {
            get { return record; }
            set { record = value; }
        }

        private string tableName;
        private SqlCommand command;

        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        public InsertCommand(string tableName, string connectionString)
        {
            command = new SqlCommand();
            command.Connection = new SqlConnection(connectionString);

            this.tableName = tableName;
            Table table = new Table(tableName, connectionString);
            string[] columnNames = table.GetColumnNames();
            this.record = new Record();
            //  一系列Column构成一条数据。初始化一条数据，值为null。
            for (int i = 0; i < columnNames.Length; i++)
            {
                this.record.Columns.Add(new Column(columnNames[i]));
            }

        }

        /// <summary>
        /// 构造对象。
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="columnNames">表列，指定要写入数据的列名数组</param>
        public InsertCommand(string tableName, string connectionString, params string[] columnNames)
        {
            command = new SqlCommand();
            command.Connection = new SqlConnection(connectionString);

            this.tableName = tableName;
          
            this.record = new Record();
            //  一系列Column构成一条数据。初始化一条数据，值为null。
            for (int i = 0; i < columnNames.Length; i++)
            {
                this.record.Columns.Add(new Column(columnNames[i]));
            }

        }

        /// <summary>
        /// 注意，该方法不能首先调用，在调用之前应先为各字段赋值，或调用AddPerameter()方法。
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            int effect = 0;
            string sql = "insert into " + tableName + " (";
            string valueSql = "";
            string parameterName = "";
            string id = "";
            for (int i = 0; i < this.record.Columns.Count; i++)
            {
                //值不为null的字段表示已赋值，是要往数据库写入的数据
                if (record.Columns[i].Value != null)
                {
                    sql += record.Columns[i].Name + ",";
                    id = DateTime.Now.ToFileTime().ToString();
                    parameterName = "@" + record.Columns[i].Name + "_" + id;
                    valueSql += parameterName + ",";
                    command.Parameters.AddWithValue(parameterName, record.Columns[i].Value);
                }
            }
            sql = sql.Remove(sql.Length - 1, 1);
            valueSql = valueSql.Remove(valueSql.Length - 1, 1);
            sql += ") values (";
            sql += valueSql;
            sql += ")";
            command.CommandText = sql;
            command.Connection.Open();
            effect = command.ExecuteNonQuery();
            command.Connection.Close();
            return effect;
        }

        /// <summary>
        /// 向数据库写入数据
        /// </summary>
        /// <param name="columnNames">写入的列名数组</param>
        /// <param name="values">对应列的值的数组</param>
        /// <returns></returns>
        public int Insert(string[] columnNames, object[] values)
        {
            int effect = 0;
            string sql = "insert into " + tableName + " (";
            string valueSql = "";
            string parameterName = "";
            string id = "";
            for (int i = 0; i < columnNames.Length; i++)
            {
                id = DateTime.Now.ToFileTime().ToString();
                sql += columnNames[i] + ",";
                parameterName = "@" + columnNames[i] + "_" + id;
                valueSql += parameterName + ",";
                command.Parameters.AddWithValue(parameterName, values[i]);
            }
            sql = sql.Remove(sql.Length - 1, 1);
            valueSql = valueSql.Remove(valueSql.Length - 1, 1);
            sql += ") values (";
            sql += valueSql;
            sql += ")";
            command.CommandText = sql;
            command.Connection.Open();
            effect = command.ExecuteNonQuery();
            command.Connection.Close();
            return effect;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public InsertCommand AddParameter(string columnName, object value)
        {
            record[columnName].Value = value;
            return this;
        }

    }
}
