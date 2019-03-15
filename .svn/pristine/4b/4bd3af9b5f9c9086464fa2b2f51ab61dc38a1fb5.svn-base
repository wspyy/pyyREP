using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace Bll
{

    /// <summary>
    /// 使用数据工厂的模式的程序
    /// </summary>
    public class SQLHelper
    {
        #region  数据库初始化和公共的变量

        public string ProviderName
        {
            get { return myProviderName; }
            set { myProviderName = value; }
        }
        private string myProviderName = "ProviderName";
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string myConnectionString = "";
        public string ConnectionString
        {
            get { return myConnectionString; }
            set { myConnectionString = value; }
        }
        private string myConnectionName = "MetaDB";
        
        public SQLHelper()
        {
            myProviderName = ConfigurationManager.ConnectionStrings[myConnectionName].ProviderName;
            myConnectionString = ConfigurationManager.ConnectionStrings[myConnectionName].ConnectionString;
        }
        /// <summary>
        /// 外部连接字符串
        /// </summary>
        public static string GetConnString()
        {
            return ConfigurationManager.ConnectionStrings["MetaDB"].ConnectionString;
        }
        /// <summary>
        /// 根据connname获取数据
        /// </summary>
        /// <param name="connName"></param>
        public SQLHelper(string connName)
        {
            myConnectionName = connName;
            myProviderName = ConfigurationManager.ConnectionStrings[myConnectionName].ProviderName;
            myConnectionString = ConfigurationManager.ConnectionStrings[myConnectionName].ConnectionString;
            //myProviderName = System.Configuration.ConfigurationSettings.AppSettings[connName];
            //myConnectionString = System.Configuration.ConfigurationSettings.AppSettings[connName];
        }
        /// <summary>
        /// 根据ProviderName和连接串获取数据
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connString"></param>
        public SQLHelper(string connString, string providerName)
        {
            myProviderName = providerName;
            myConnectionString = connString;
        }
        /// <summary>
        /// 静态方法，返回DbParameter
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateParameter()
        {
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            return myDataFactory.CreateParameter();
        }
        public DbParameter CreateParameter(string dbPararName, DbType dbType, object value)
        {
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            DbParameter result = myDataFactory.CreateParameter();
            result.ParameterName = dbPararName;
            result.DbType = dbType;          
            if (value == null)
            {
                result.Value = DBNull.Value;
            }
            else
            {
                result.Value = value;
            }
            return result;
        }
        /// <summary>
        /// 静态方法，返回DbCommandBuilder
        /// </summary>
        /// <returns></returns>
        public DbCommandBuilder CreateCommandBuilder()
        {
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            return myDataFactory.CreateCommandBuilder();
        }
        /// <summary>
        /// 静态方法，返回DbConnection
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
        {
            DbConnection myResult;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);

            myResult = myDataFactory.CreateConnection();
            myResult.ConnectionString = myConnectionString;
            return myResult;
        }
        /// <summary>
        /// 静态方法，返回DbCommandBuilder
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter CreateDataAdapter()
        {
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            return myDataFactory.CreateDataAdapter();
        }

        #endregion

        #region  执行SQL语句的所有的方法

        public bool TestConnection()
        {
            bool fResult = false;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);

            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    fResult = true;
                }
                catch (Exception ex)
                {

                }
            }

            return fResult;
        }

        public string GetDataBaseName()
        {
            string fResult = "DataCenter";
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);

            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = myConnectionString;
                    conn.Open();
                    fResult = conn.Database;
                }
                catch (Exception ex)
                {

                }
            }

            return fResult;

        }

        ///	<summary>
        ///	创建日期：2005-4-6
        ///	传入参数：语句，参数数组
        ///	操作目的：执行存储过程
        ///	返 回 值：执行的行数
        ///	创 建 人：宋桂祥
        ///	</summary>
        ///	<param name="sProcTxt">语句</param>
        ///	<param name="parameters">参数数组</param>
        ///	<returns>
        ///	返 回 值：执行的行数
        ///	</returns>
        ///	<remarks>
        ///	
        ///	</remarks>
        public DataSet ExecuteSQLDataSet(string sSQLTxt)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);

            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    comm.CommandTimeout = 300;
                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                        //throw new Exception();
                    }
                    finally
                    {
                        if (conn != null) conn.Close();
                    }
                }
            }

            return fResult;
        }
        public DataSet ExecuteSQLDataSet(string sSQLTxt, int iStart, int iPageSize)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    comm.CommandTimeout = 100;
                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, (iStart - 1) * iPageSize, iPageSize, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        //self
        public  DataTable GetDataTable(string commTxt, CommandType type, params SqlParameter[] ps)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(commTxt, myConnectionString))
            {
                if (ps.Length > 0)
                {
                    adapter.SelectCommand.Parameters.AddRange(ps);
                }
                adapter.SelectCommand.CommandType = type;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
        public DataSet ExecuteSQLDataSet(string sSQLTxt, List<DbParameter> parameters)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;

                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }

        /// <summary>
        /// 执行语句或存储过程
        /// </summary>
        /// <param name="sSQLTxt">SQL语句或存储过程</param>
        /// <param name="comType">CommandType枚举</param>
        /// <param name="parameters">参数集</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteSQLDataSet(string sSQLTxt,CommandType comType, List<DbParameter> parameters)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandTimeout = 12000;
                    comm.CommandType = comType;

                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }

        public DataSet ExecuteSQLDataSet(string sSQLTxt, List<DbParameter> parameters, int iStart, int iPageSize)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;

                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, (iStart - 1) * iPageSize, iPageSize, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }

        public DataSet ExecuteSQLDataSet(string sSQLTxt, List<DbParameter> parameters,CommandType commtype, int iStart, int iPageSize)
        {
            DataSet fResult = new DataSet();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataAdapter adap = myDataFactory.CreateDataAdapter();
                    adap.SelectCommand = comm;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = commtype;

                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行               
                        adap.Fill(fResult, (iStart - 1) * iPageSize, iPageSize, "DataTable");
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }

        ///	<summary>
        ///	创建日期：2005-4-6
        ///	传入参数：语句，参数数组
        ///	操作目的：执行存储过程
        ///	返 回 值：执行的行数
        ///	创 建 人：宋桂祥
        ///	</summary>
        ///	<param name="sProcTxt">语句</param>
        ///	<param name="parameters">参数数组</param>
        ///	<returns>
        ///	返 回 值：执行的行数
        ///	</returns>
        ///	<remarks>
        ///	
        ///	</remarks>
        public bool ExecuteSQLExist(string sSQLTxt)
        {
            bool fResult = false;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataReader dr;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    try
                    {
                        //打开连接
                        conn.Open();
                        dr = comm.ExecuteReader();
                        if (dr.Read())
                        {
                            fResult = true;
                        }
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        public bool ExecuteSQLExist(string sSQLTxt, List<DbParameter> parameters)
        {
            bool fResult = false;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    DbDataReader dr;
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行
                        conn.Open();
                        dr = comm.ExecuteReader();
                        if (dr.Read())
                        {
                            fResult = true;
                        }
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        ///	<summary>
        ///	创建日期：2005-4-6
        ///	传入参数：语句，参数数组
        ///	操作目的：执行存储过程
        ///	返 回 值：执行的行数
        ///	创 建 人：宋桂祥
        ///	</summary>
        ///	<param name="sProcTxt">语句</param>
        ///	<returns>
        ///	返 回 值：执行的行数
        ///	</returns>
        ///	<remarks>
        ///	
        ///	</remarks>
        public int ExecuteSQLNonQuery(string sSQLTxt)
        {
            int fResult = 0;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    comm.CommandTimeout = 300;
                    //打开连接
                    //try
                    //{
                        //执行
                        conn.Open();
                        fResult = comm.ExecuteNonQuery();
                    //}
                    //catch (System.Data.Common.DbException ex)
                    //{
                        //PublicUtility.HandException("DbBase.cs", ex);
                    //}
                    //finally
                    //{
                        //关闭连接
                        comm.Parameters.Clear();
                        if (conn != null) conn.Close();
                    //}
                }
            }
            return fResult;
        }
        public int ExecuteSQLNonQuery(string sSQLTxt, List<DbParameter> parameters)
        {
            int fResult = 0;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    //给Command添加参数
                    PrepareParameters(comm, parameters);

                    //打开连接
                    try
                    {
                        //执行
                        conn.Open();
                        fResult = comm.ExecuteNonQuery();
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        ///	<summary>
        ///	创建日期：2005-4-6
        ///	传入参数：语句，参数数组
        ///	操作目的：执行存储过程
        ///	返 回 值：执行的行数
        ///	创 建 人：宋桂祥
        ///	</summary>
        ///	<param name="sProcTxt">语句</param>
        ///	<param name="parameters">参数数组</param>
        ///	<returns>
        ///	返 回 值：执行的行数
        ///	</returns>
        ///	<remarks>
        ///	
        ///	</remarks>
        public object ExecuteSQLScalar(string sSQLTxt)
        {
            object fResult = null;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    try
                    {
                        //打开连接
                        conn.Open();
                        fResult = comm.ExecuteScalar();
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        public object ExecuteSQLScalar(string sSQLTxt, List<DbParameter> parameters)
        {
            object fResult = null;
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {
                using (DbCommand comm = conn.CreateCommand())
                {
                    conn.ConnectionString = myConnectionString;
                    comm.CommandText = sSQLTxt;
                    comm.CommandType = CommandType.Text;
                    //给Command添加参数
                    PrepareParameters(comm, parameters);
                    //打开连接
                    try
                    {
                        //执行
                        conn.Open();
                        fResult = comm.ExecuteScalar();
                    }
                    catch (System.Data.Common.DbException ex)
                    {
                        //PublicUtility.HandException("DbBase.cs", ex);
                    }
                    finally
                    {
                        //关闭连接
                        comm.Parameters.Clear();

                        if (conn != null) conn.Close();
                    }
                }
            }
            return fResult;
        }
        /// <summary>
        /// 准备参数
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="parameters"></param>
        private void PrepareParameters(DbCommand comm, List<DbParameter> parameters)
        {
            //先清空原有的参数
            comm.Parameters.Clear();
            //给Command添加参数
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.Value == null)
                {
                    parameter.Value = DBNull.Value;
                }

                if (parameter.DbType == DbType.Guid)
                {
                    parameter.Value = new Guid(parameter.Value.ToString());
                }

                else if (parameter.DbType == DbType.Boolean)
                {
                    if (parameter.Value.ToString() == "on")
                    {
                        parameter.Value = true ;
                    }
                }

                comm.Parameters.Add(parameter);
            }
        }

        #endregion  执行SQL语句的所有的方法

        #region  返回所有的架构信息
        public DataTable GetSchema()
        {
            DataTable fResult = new DataTable();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {

                conn.ConnectionString = myConnectionString;
                try
                {
                    conn.Open();
                    fResult = conn.GetSchema();
                }
                catch (System.Data.Common.DbException ex)
                {
                    //PublicUtility.HandException("DbBase.cs", ex);
                }
                finally
                {
                    if (conn != null) conn.Close();
                }
            }
            return fResult;
        }

        public DataTable GetSchema(string collectionName)
        {
            DataTable fResult = new DataTable();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {

                conn.ConnectionString = myConnectionString;
                try
                {
                    conn.Open();
                    fResult = conn.GetSchema(collectionName);
                }
                catch (System.Data.Common.DbException ex)
                {
                    //PublicUtility.HandException("DbBase.cs", ex);
                }
                finally
                {
                    if (conn != null) conn.Close();
                }
            }
            return fResult;
        }

        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            DataTable fResult = new DataTable();
            DbProviderFactory myDataFactory = DbProviderFactories.GetFactory(myProviderName);
            using (DbConnection conn = myDataFactory.CreateConnection())
            {

                conn.ConnectionString = myConnectionString;
                try
                {
                    conn.Open();
                    fResult = conn.GetSchema(collectionName, restrictionValues);
                }
                catch (System.Data.Common.DbException ex)
                {
                    //PublicUtility.HandException("DbBase.cs", ex);
                }
                finally
                {
                    if (conn != null) conn.Close();
                }
            }
            return fResult;
        }
        #endregion  返回所有的架构信息

        #region 批量导入数据

        /// <summary>
        /// 将DataTable的数据批量插入到数据库中。
        /// </summary>
        /// <param name="dataTable">要批量插入的DataTable</param>
        /// <param name="tableName">数据库中对应的表,注意表的拥有者</param>
        /// <param name="batchSize">批次的数目</param>
        /// <returns></returns>
        public bool InsertBatchDataTable(DataTable dataTable, string tableName, int batchSize = 10000)
        {
            using (SqlConnection connection = new SqlConnection(myConnectionString))
            {
                try
                {
                    connection.Open();
                    //给表名加上前后导符
                    using (var bulk = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity, null)
                    {
                        DestinationTableName = tableName,
                        BatchSize = batchSize
                    })
                    {
                        //循环所有列，为bulk添加映射
                        //dataTable.EachColumn(c => bulk.ColumnMappings.Add(c.ColumnName, c.ColumnName), c => !c.AutoIncrement);
                        foreach (DataColumn dc in dataTable.Columns)
                        {
                            bulk.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                        }
                        bulk.WriteToServer(dataTable);
                        bulk.Close();
                    }
                    return true;
                }
                catch (Exception exp)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion
        /// <summary> 
        /// 执行指定数据库连接字符串的命令,返回DataSet. 
        /// </summary> 
        /// <remarks> 
        /// 示例: 
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connectionString">一个有效的数据库连接字符串</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名称或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamters参数数组</param> 
        /// <returns>返回一个包含结果集的DataSet</returns> 
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
            {
                throw new ArgumentNullException("connectionString");
            }

            // 创建并打开数据库连接对象,操作完成释放对象. 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    throw;
                }
                // 调用指定数据库连接字符串重载方法. 
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }

        }
        /// <summary> 
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet. 
        /// </summary> 
        /// <remarks> 
        /// 示例:  
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个有效的数据库连接对象</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param> 
        /// <param name="commandText">存储过程名或T-SQL语句</param> 
        /// <param name="commandParameters">SqlParamter参数数组</param> 
        /// <returns>返回一个包含结果集的DataSet</returns> 
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters,out mustCloseConnection);
        using(SqlDataAdapter da=new SqlDataAdapter(cmd))
        {
            cmd.CommandTimeout =600;
            DataSet ds=new DataSet();
             da.Fill(ds);
            cmd.Parameters.Clear();
            if(mustCloseConnection)
                connection.Close();
            return ds;
        }
        }
        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数 
        /// </summary> 
        /// <param name="command">要处理的SqlCommand</param> 
        /// <param name="connection">数据库连接</param> 
        /// <param name="transaction">一个有效的事务或者是null值</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param> 
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param> 
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param> 
        private static void PrepareCommand(SqlCommand command, SqlConnection connecion, SqlTransaction transaction, CommandType commandtype, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
            if (connecion.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                try
                {
                    connecion.Open();
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connecion;
            command.CommandText = commandText;
            command.CommandTimeout =600;
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }
            command.CommandType = commandtype;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        /// <summary> 
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令. 
        /// 这个方法将给任何一个参数分配DBNull.Value; 
        /// 该操作将阻止默认值的使用. 
        /// </summary> 
        /// <param name="command">命令名</param> 
        /// <param name="commandParameters">SqlParameters数组</param> 
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value=DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
    }
}

