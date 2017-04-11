using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    public class Table
    {
        //private List<Record> rows = new List<Record>();

        //public List<Record> Rows
        //{
        //    get { return rows; }
        //    set { rows = value; }
        //}

        //public Record this[int index]
        //{
        //    get
        //    {
        //        return rows[index];
        //    }
        //}

        // 该语句读取SqlServer表的所有字段(列)名
        private string columnQuerySql = @"SELECT sys.syscolumns.name
                            FROM sys.syscolumns INNER JOIN
                            sys.sysobjects ON sys.syscolumns.id = sys.sysobjects.id AND sys.sysobjects.name = '{0}' AND 
                            sys.sysobjects.xtype = 'U' ";

        /// <summary>
        /// 获取当前表的所有列名，以数组形式返回。
        /// </summary>
        /// <returns></returns>
        public string[] GetColumnNames()
        {
            List<string> colNames = new List<string>();
            columnQuerySql = string.Format(columnQuerySql, tableName);
            DB db = new DB(connectionString);
            object[][] fields = db.GetDataTableArray(columnQuerySql);
            for (int i = 0; i < fields.Length; i++)
            {
                colNames.Add(fields[i][0].ToString());
            }
            return colNames.ToArray();
        }

        private string tableName;
        private string connectionString;

        public Table(string tableName, string connectionString)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
        }
    }
}
