using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GShine.Data
{
    /// <summary>
    /// 提供对数据库的基础访问
    /// </summary>
    public class DB
    {
        private string connectionString;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        /// <summary>
        /// 使用配置连接字符串索引初始化对象。
        /// </summary>
        /// <param name="connectionStringIndex">连接字符串在配置文件中的索引</param>
        public DB(int connectionStringIndex)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString;
        }


        /// <summary>
        /// 直接使用连接字符串初始化对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public DB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 使用指定SqlCommand查询表数据。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable GetDataTable(SqlCommand command)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            return table;
        }

        /// <summary>
        /// 根据查询语句获得结果DataTable
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(sql,connectionString);
            adapter.Fill(table);
            return table;
        }

        /// <summary>
        /// 查询并获得DataTable
        /// </summary>
        /// <param name="sql">查询语句，参数格式： @0,@1...，从0开始，后面的值数组要与之对应。</param>
        /// <param name="values">对应参数的值</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, params object[] values)
        {
            SqlCommand command = new SqlCommand(sql);
            command.Connection = new SqlConnection(connectionString);
            for (int i = 0; i < values.Length; i++)
            {
                command.Parameters.AddWithValue("@"+i.ToString(),values[i]);
            }
           
            SqlDataAdapter adp = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adp.Fill(table);
            return table;
        }

        private object[][] DataTableToArray(DataTable table)
        {
            List<object[]> rows = new List<object[]>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                object[] row = new object[table.Columns.Count];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    row[j] = table.Rows[i][j];
                }
                rows.Add(row);
            }
            return rows.ToArray();
        }

        /// <summary>
        /// 根据指定的SqlCommand对象查询数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object[][] GetDataTableArray(SqlCommand command)
        {
            List<object[]> rows = new List<object[]>();
            DataTable table = GetDataTable(command);
            return DataTableToArray(table);
        }

        /// <summary>
        /// 根据查询语句获得结果数组。查询出的结果有可能为DBNull.Value，注意判断。
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public object[][] GetDataTableArray(string sql)
        {
            List<object[]> rows = new List<object[]>();
            DataTable table = GetDataTable(sql);
            return DataTableToArray(table);
        }

        /// <summary>
        /// 有参数的查询
        /// </summary>
        /// <param name="sql">带参数的查询语句，其格式为：@0，@1...</param>
        /// <param name="values">与参数对应的值</param>
        /// <returns></returns>
        public object[][] GetDataTableArray(string sql, params object[] values)
        {
            List<object[]> rows = new List<object[]>();
            DataTable table = GetDataTable(sql,values);
            return DataTableToArray(table);
        }

    }
}
