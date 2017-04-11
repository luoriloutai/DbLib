using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GShine.Data
{
    /// <summary>
    /// 删除命令
    /// </summary>
    public class DeleteCommand : Command
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        public DeleteCommand(string tableName, string connectionString)
            : base(tableName,connectionString)
        {
            string delSql = "delete from " + tableName + " where 1=1 ";
            this.SQLCommand.CommandText = delSql;
        }

        /// <summary>
        /// 执行删除操作，返回受影响行数。默认删除全部数据，如果想删除指定条件数据，请先调用Where、LikeWhere方法。
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            int effectCount=0;
            SQLCommand.CommandText += WhereString;
            SQLCommand.Connection.Open();
            effectCount = this.SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effectCount;
        }

        /// <summary>
        /// 指定条件语句删除数据，该方法不需调用Where、LikeWhere等方法，过滤条件以参数where指定。
        /// </summary>
        /// <param name="where">条件，注意条件连接时需要带“and”、"or"等关键字。</param>
        /// <returns></returns>
        public int Delete(string where)
        {
            int effectCount = 0;
            SQLCommand.CommandText += where;
            SQLCommand.Connection.Open();
            effectCount = this.SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effectCount;
        }

        /// <summary>
        /// 带参数的删除
        /// </summary>
        /// <param name="where">条件语句，注意条件连接时需要带“and”、"or"等关键字，参数格式为：@0、@1，从@0开始</param>
        /// <param name="values">对应参数的值</param>
        /// <returns></returns>
        public int Delete(string where, params object[] values)
        {
            int effectCount = 0;
            SQLCommand.CommandText += where;

            for (int i = 0; i < values.Length; i++)
            {
                SQLCommand.Parameters.AddWithValue("@" + i.ToString(), values[i]);
            }

            SQLCommand.Connection.Open();
            effectCount = this.SQLCommand.ExecuteNonQuery();
            SQLCommand.Connection.Close();
            return effectCount;
        }

        /// <summary>
        /// 指定Where条件，该方法可以连续调用。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <param name="value">列对应的值</param>
        /// <returns></returns>
        public new DeleteCommand Where(LogicOperator logicOperator, string columnName, ValueOperator valueOperator, object value)
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
        public new DeleteCommand LikeWhere(LogicOperator logicOperator, string columnName, object value, Wildcard position)
        {
            base.LikeWhere(logicOperator, columnName, value, position);
            return this;
        }


        /// <summary>
        /// 指定某列值为NULL。该方法可连续调用。由于使用Where方法指定某列值为null(如：columnName is @arg，@arg为null)时会出错，所以有了该方法。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <returns></returns>
        public new DeleteCommand NullWhere(LogicOperator logicOperator, string columnName, NullValueOperator valueOperator)
        {
             base.NullWhere(logicOperator, columnName, valueOperator);
             return this;
        }

        /// <summary>
        /// 分组左包裹，分组查询左半个括号。该方法与GroupWrapRight()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <returns></returns>
        public new DeleteCommand GroupWrapLeft(LogicOperator logicOperator)
        {
            base.GroupWrapLeft(logicOperator);
            return this;
        }

        /// <summary>
        /// 分组右包裹，分组查询右半个括号。该方法与GroupWrapLeft()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <returns></returns>
        public new DeleteCommand GroupWrapRight()
        {
            base.GroupWrapRight();
            return this;
        }

    }
}
