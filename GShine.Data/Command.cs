using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace GShine.Data
{
    public class Command
    {
        private string tableName = "";

        /// <summary>
        /// 表名
        /// </summary>
        protected string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        private string connectionString = "";

        /// <summary>
        /// 连接字符串
        /// </summary>
        protected string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        /// 使用连接字符串和表名初始化对象
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="connectionString">连接字符串</param>
        protected Command(string tableName, string connectionString)
        {
            this.tableName = tableName;
            this.connectionString = connectionString;
            sQLCommand = new SqlCommand();
            sQLCommand.Connection = new SqlConnection(connectionString);
        }


        /// <summary>
        /// 使用连接字符串初始化对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        protected Command(string connectionString)
        {
            this.connectionString = connectionString;
            sQLCommand = new SqlCommand();
            sQLCommand.Connection = new SqlConnection(connectionString);
        }


        private SqlCommand sQLCommand;

        /// <summary>
        /// 执行Sql所需的SqlCommand
        /// </summary>
        public SqlCommand SQLCommand
        {
            get { return sQLCommand; }
            set { sQLCommand = value; }
        }

        private string whereString = "";

        /// <summary>
        /// 查询的Where条件子句
        /// </summary>
        protected string WhereString
        {
            get { return whereString; }
            set { whereString = value; }
        }

        /// <summary>
        /// Like查询时获取Like后的语句
        /// </summary>
        /// <param name="columnName">列名</param>
        /// <param name="value">值</param>
        /// <param name="position">通配符的位置</param>
        /// <returns></returns>
        protected string GetLikeString(string columnName, object value, Wildcard position)
        {
            string id = DateTime.Now.ToFileTime().ToString();
            string parameterName = "@" + columnName + "_" + id;
            string likeStr = "";

            switch (position)
            {
                case Wildcard.All:
                    likeStr = string.Format("'%'+{0}+'%' ", parameterName);
                    break;
                case Wildcard.Front:
                    likeStr = string.Format("'%'+{0} ", parameterName);
                    break;
                case Wildcard.Back:
                    likeStr = string.Format("{0}+'%' ", parameterName);
                    break;
            }
            sQLCommand.Parameters.AddWithValue(parameterName, value);
            return likeStr;
        }

        private string GetLogicOperator(LogicOperator op)
        {
            string opStr = "";
            switch (op)
            {
                case LogicOperator.And:
                    opStr = " and ";
                    break;
                case LogicOperator.Or:
                    opStr = " or ";
                    break;
            }
            return opStr;
        }

        private string GetValueOperator(ValueOperator op)
        {
            string opStr = "";
            switch (op)
            {
                case ValueOperator.Equal:
                    opStr = " = ";
                    break;
                case ValueOperator.Greater:
                    opStr = " > ";
                    break;
                case ValueOperator.GreaterEqual:
                    opStr = " >= ";
                    break;
                case ValueOperator.Less:
                    opStr = " < ";
                    break;
                case ValueOperator.LessEqual:
                    opStr = " <= ";
                    break;
                case ValueOperator.NotEqual:
                    opStr = " <> ";
                    break;
            }
            return opStr;
        }

        /// <summary>
        /// 为查询指定Where条件，该方法可以连续调用。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <param name="value">列对应的值</param>
        /// <returns></returns>
        public Command Where(LogicOperator logicOperator, string columnName, ValueOperator valueOperator, object value)
        {
            //使用id目的是为了避免参数名重复
            string id = DateTime.Now.ToFileTime().ToString();
            string parameterName = "@" + columnName + "_" + id;
            whereString += GetLogicOperator(logicOperator) + columnName + GetValueOperator(valueOperator) + parameterName + " ";
            sQLCommand.Parameters.AddWithValue(parameterName, value);
            return this;
        }

        /// <summary>
        ///为查询指定Like条件，该方法可以连续调用
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="value">列对应的值</param>
        /// <param name="position">通配符位置</param>
        /// <returns></returns>
        public Command LikeWhere(LogicOperator logicOperator, string columnName, object value, Wildcard position)
        {
            whereString += GetLogicOperator(logicOperator) + columnName + " like " + this.GetLikeString(columnName, value, position);
            return this;
        }

        private string GetNullValueOperator(NullValueOperator op)
        {
            string opStr = "";
            switch (op)
            {
                case NullValueOperator.Is:
                    opStr = " is ";
                    break;
                case NullValueOperator.IsNot:
                    opStr = " is not ";
                    break;
            }
            return opStr;
        }

        /// <summary>
        /// 指定某列值为NULL的查询。该方法可连续调用。由于使用Where方法指定某列值为null(如：columnName is @arg，@arg为null)时会出错，所以有了该方法。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <param name="columnName">列名</param>
        /// <param name="valueOperator">值操作符</param>
        /// <returns></returns>
        public Command NullWhere(LogicOperator logicOperator, string columnName, NullValueOperator valueOperator)
        {
            whereString += GetLogicOperator(logicOperator) + columnName + GetNullValueOperator(valueOperator) + " null ";
            return this;
        }

        /// <summary>
        /// 分组左包裹，分组查询左半个括号。该方法与GroupWrapRight()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <param name="logicOperator">逻辑操作符</param>
        /// <returns></returns>
        public Command GroupWrapLeft(LogicOperator logicOperator)
        {
            whereString += GetLogicOperator(logicOperator) + "( ";
            return this;
        }

        /// <summary>
        /// 分组右包裹，分组查询右半个括号。该方法与GroupWrapLeft()方法成对使用，构成一个完整的分组查询条件。
        /// </summary>
        /// <returns></returns>
        public Command GroupWrapRight()
        {
            whereString += " ) ";
            return this;
        }

    }
}
