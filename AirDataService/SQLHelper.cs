using System;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
using System.Collections;
/// <summary>
/// SqlServer���ݷ��ʰ�����
/// </summary>
public sealed class SqlHelper
{
    #region ˽�й��캯���ͷ���
    private SqlHelper() { }
    /// <summary>
    /// ��SqlParameter��������(����ֵ)�����SqlCommand����.
    /// ������������κ�һ����������DBNull.Value;
    /// �ò�������ֹĬ��ֵ��ʹ��.
    /// </summary>
    /// <param name="command">������</param>
    /// <param name="commandParameters">SqlParameters����</param>
    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandParameters != null)
        {
            foreach (SqlParameter p in commandParameters)
            {
                if (p != null)
                {
                    // ���δ����ֵ���������,���������DBNull.Value.
                    if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                        (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }
                    command.Parameters.Add(p);
                }
            }
        }
    }

    /// <summary>
    /// ��DataRow���͵���ֵ���䵽SqlParameter��������.
    /// </summary>
    /// <param name="commandParameters">Ҫ����ֵ��SqlParameter��������</param>
    /// <param name="dataRow">��Ҫ������洢���̲�����DataRow</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
    {
        if ((commandParameters == null) || (dataRow == null))
        {
            return;
        }
        int i = 0;
        // ���ò���ֵ
        foreach (SqlParameter commandParameter in commandParameters)
        {
            // ������������,���������,ֻ�׳�һ���쳣.
            if (commandParameter.ParameterName == null ||
                commandParameter.ParameterName.Length <= 1)
                throw new Exception(
                    string.Format("���ṩ����{0}һ����Ч������{1}.", i, commandParameter.ParameterName));
            // ��dataRow�ı��л�ȡΪ�����������������Ƶ��е�����.
            // ������ںͲ���������ͬ����,����ֵ������ǰ���ƵĲ���.
            if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
            i++;
        }
    }

    /// <summary>
    /// ��һ��������������SqlParameter��������.
    /// </summary>
    /// <param name="commandParameters">Ҫ����ֵ��SqlParameter��������</param>
    /// <param name="parameterValues">��Ҫ������洢���̲����Ķ�������</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        if ((commandParameters == null) || (parameterValues == null))
        {
            return;
        }
        // ȷ����������������������ƥ��,�����ƥ��,�׳�һ���쳣.
        if (commandParameters.Length != parameterValues.Length)
        {
            throw new ArgumentException("����ֵ�����������ƥ��.");
        }
        // ��������ֵ
        for (int i = 0, j = commandParameters.Length; i < j; i++)
        {
            // If the current array value derives from IDbDataParameter, then assign its Value property
            if (parameterValues[i] is IDbDataParameter)
            {
                IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                if (paramInstance.Value == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = paramInstance.Value;
                }
            }
            else if (parameterValues[i] == null)
            {
                commandParameters[i].Value = DBNull.Value;
            }
            else
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }
    }

    /// <summary>
    /// Ԥ�����û��ṩ������,���ݿ�����/����/��������/����
    /// </summary>
    /// <param name="command">Ҫ�����SqlCommand</param>
    /// <param name="connection">���ݿ�����</param>
    /// <param name="transaction">һ����Ч�����������nullֵ</param>
    /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
    /// <param name="commandText">�洢��������T-SQL�����ı�</param>
    /// <param name="commandParameters">�������������SqlParameter��������,���û�в���Ϊ'null'</param>
    /// <param name="mustCloseConnection"><c>true</c> ��������Ǵ򿪵�,��Ϊtrue,���������Ϊfalse.</param>
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
    {
        if (command == null) throw new ArgumentNullException("command");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
        // If the provided connection is not open, we will open it
        if (connection.State != ConnectionState.Open)
        {
            mustCloseConnection = true;
            connection.Open();
        }
        else
        {
            mustCloseConnection = false;
        }
        // ���������һ�����ݿ�����.
        command.Connection = connection;
        // ���������ı�(�洢��������SQL���)
        command.CommandText = commandText;
        // ��������
        if (transaction != null)
        {
            if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            command.Transaction = transaction;
        }
        // ������������.
        command.CommandType = commandType;
        // �����������
        if (commandParameters != null)
        {
            AttachParameters(command, commandParameters);
        }
        return;
    }
    #endregion ˽�й��캯���ͷ�������

    #region ExecuteNonQuery����
    /// <summary>
    /// ִ��ָ�������ַ���,���͵�SqlCommand.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
    /// <param name="commandText">�洢�������ƻ�SQL���</param>
    /// <returns>��������Ӱ�������</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�������ַ���,���͵�SqlCommand.���û���ṩ����,�����ؽ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�, ����.)</param>
    /// <param name="commandText">�洢�������ƻ�SQL���</param>
    /// <param name="commandParameters">SqlParameter��������</param>
    /// <returns>��������Ӱ�������</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// ִ��ָ�������ַ����Ĵ洢����,�����������ֵ�����洢���̲���,
    /// �˷�����Ҫ�ڲ������淽����̽�����������ɲ���.
    /// </summary>
    /// <remarks>
    /// �������û���ṩ������������ͷ���ֵ.
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(connString, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���/param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">���䵽�洢������������Ķ�������</param>
    /// <returns>������Ӱ�������</returns>
    public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ������ڲ���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // ��̽���洢���̲���(���ص�����)��������洢���̲�������.
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в��������
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ�������� 
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
    /// <param name="commandText">T�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">SqlParamter��������</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        // ����SqlCommand����,������Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // Finally, execute the command
        int retval = cmd.ExecuteNonQuery();

        // �������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        if (mustCloseConnection)
            connection.Close();
        return retval;
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,�����������ֵ�����洢���̲���.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(conn, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ���洢���̷������ֵ
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ�д������SqlCommand.
    /// </summary>
    /// <remarks>
    /// ʾ��.:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders");
    /// </remarks>
    /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>����Ӱ�������/returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ�д������SqlCommand(ָ������).
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">��������(�洢����,�����ı�������.)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">SqlParamter��������</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        // Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ��
        int retval = cmd.ExecuteNonQuery();

        // ���������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// ִ�д������SqlCommand(ָ������ֵ).
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ
    /// ʾ��:  
    ///  int result = ExecuteNonQuery(conn, trans, "PublishOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>������Ӱ�������</returns>
    public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion ExecuteNonQuery��������

    #region ��ҳ
    public static DataTable GetCurrentPage(int pageIndex, int pageSize, string sqlStr, string connectionString)
    {
        DataTable dt = new DataTable();
        //���õ������ʼ��ַ
        int firstPage = pageIndex * pageSize;

        SqlConnection con = new SqlConnection(connectionString);
        con.Open();
        SqlCommand com = new SqlCommand(sqlStr, con);
        DataSet dataset = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
        dataAdapter.Fill(dataset, firstPage, pageSize, "outputsell");
        com.Dispose();
        con.Close();
        dataAdapter.Dispose();
        dt = dataset.Tables["outputsell"];
        return dt;
    }
    #endregion
    #region ExecuteDataset����
    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��: 
    ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">SqlParamters��������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        // �����������ݿ����Ӷ���,��������ͷŶ���.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // ����ָ�����ݿ������ַ������ط���.
            return ExecuteDataset(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ֱ���ṩ����ֵ,����DataSet.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
    /// ʾ��: 
    ///  DataSet ds = ExecuteDataset(connString, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м����洢���̲���
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            // ���洢���̲�������ֵ
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ָ���洢���̲���,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <param name="commandParameters">SqlParamter��������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        // Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // ����SqlDataAdapter��DataSet.
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();
            // ���DataSet.
            da.Fill(ds);

            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return ds;
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,����DataSet.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
    /// ʾ��.:  
    ///  DataSet ds = ExecuteDataset(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �Ȼ����м��ش洢���̲���
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ���洢���̲�������ֵ
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����������,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">����</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
    }
    
    /// <summary>
    /// ִ��ָ�����������,ָ������,����DataSet.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">����</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <param name="commandParameters">SqlParamter��������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        // Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ���� DataAdapter & DataSet
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }
    }

    /// <summary>
    /// ִ��ָ�����������,ָ������ֵ,����DataSet.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ.
    /// ʾ��.:  
    ///  DataSet ds = ExecuteDataset(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">����</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����һ�������������DataSet</returns>
    public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            // ���洢���̲�������ֵ
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion ExecuteDataset���ݼ��������

    #region ExecuteReader �����Ķ���
    /// <summary>
    /// ö��,��ʶ���ݿ���������SqlHelper�ṩ�����ɵ������ṩ
    /// </summary>
    private enum SqlConnectionOwnership
    {
        /// <summary>��SqlHelper�ṩ����</summary>
        Internal,
        /// <summary>�ɵ������ṩ����</summary>
        External
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
    /// </summary>
    /// <remarks>
    /// �����SqlHelper������,�����ӹر�DataReaderҲ���ر�.
    /// ����ǵ��ö�������,DataReader�ɵ��ö�����.
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="transaction">һ����Ч������,����Ϊ 'null'</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <param name="commandParameters">SqlParameters��������,���û�в�����Ϊ'null'</param>
    /// <param name="connectionOwnership">��ʶ���ݿ����Ӷ������ɵ������ṩ������SqlHelper�ṩ</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        bool mustCloseConnection = false;
        // ��������
        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ���������Ķ���
        SqlDataReader dataReader;
        if (connectionOwnership == SqlConnectionOwnership.External)
        {
            dataReader = cmd.ExecuteReader();
        }
        else
        {
            dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // �������,�Ա��ٴ�ʹ��..
        // HACK: There is a problem here, the output parameter values are fletched 
        // when the reader is closed, so if the parameters are detached from the command
        // then the SqlReader can�t set its values. 
        // When this happen, the parameters can�t be used again in other command.
        bool canClear = true;
        foreach (SqlParameter commandParameter in cmd.Parameters)
        {
            if (commandParameter.Direction != ParameterDirection.Input)
                canClear = false;
        }

        if (canClear)
        {
            cmd.Parameters.Clear();
        }
        return dataReader;

    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ����������Ķ���.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
    {
        return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <param name="commandParameters">SqlParamter��������(new SqlParameter("@prodid", 24))</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        SqlConnection connection = null;

        connection = new SqlConnection(connectionString);
        connection.Open();
        return ExecuteReader(connection, null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);

    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ����������Ķ���,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(connString, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ���������Ķ���.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢��������T-SQL���</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandParameters">SqlParamter��������</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// [�����߷�ʽ]ִ��ָ�����ݿ����Ӷ���������Ķ���,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">T�洢������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
    }

    /// <summary>
    /// [�����߷�ʽ]ִ��ָ�����ݿ�����������Ķ���,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  SqlDataReader dr = ExecuteReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            AssignParameterValues(commandParameters, parameterValues);
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion ExecuteReader�����Ķ���

    #region ExecuteScalar ���ؽ�����еĵ�һ�е�һ��
    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
    {
        // ִ�в���Ϊ�յķ���
        return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ָ������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        // �����������ݿ����Ӷ���,��������ͷŶ���.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // ����ָ�����ݿ������ַ������ط���.
            return ExecuteScalar(connection, commandType, commandText, commandParameters);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(connString, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
    {
        // ִ�в���Ϊ�յķ���
        return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ָ������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        // ����SqlCommand����,������Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ��SqlCommand����,�����ؽ��.
        object retval = cmd.ExecuteScalar();

        // �������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        if (mustCloseConnection)
            connection.Close();
        return retval;
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(conn, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        // ִ�в���Ϊ�յķ���
        return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        // ����SqlCommand����,������Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ��SqlCommand����,�����ؽ��.
        object retval = cmd.ExecuteScalar();

        // �������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,ָ������ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  int orderCount = (int)ExecuteScalar(trans, "GetOrderCount", 24, 36);
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // PPull the parameters for this stored procedure from the parameter cache ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion ExecuteScalar

    #region ExecuteXmlReader XML�Ķ���
    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
    {
        // ִ�в���Ϊ�յķ���
        return ExecuteXmlReader(connection, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        bool mustCloseConnection = false;
        // ����SqlCommand����,������Ԥ����
        SqlCommand cmd = new SqlCommand();

        PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ������
        XmlReader retval = cmd.ExecuteXmlReader();

        // �������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        return retval;

    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ����SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(conn, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢�������� using "FOR XML AUTO"</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders");
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
    {
        // ִ�в���Ϊ�յķ���
        return ExecuteXmlReader(transaction, commandType, commandText, (SqlParameter[])null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL��� using "FOR XML AUTO"</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        // ����SqlCommand����,������Ԥ����
        SqlCommand cmd = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ������
        XmlReader retval = cmd.ExecuteXmlReader();

        // �������,�Ա��ٴ�ʹ��.
        cmd.Parameters.Clear();
        return retval;
    }

    /// <summary>
    /// ִ��ָ�����ݿ������SqlCommand����,������һ��XmlReader������Ϊ���������,ָ������ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  XmlReader r = ExecuteXmlReader(trans, "GetOrders", 24, 36);
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    /// <returns>����һ�������������DataSet.</returns>
    public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            // û�в���ֵ
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion ExecuteXmlReader �Ķ�������

    #region FillDataset ������ݼ�
    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ��������ݼ�.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)</param>
    public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");

        // �����������ݿ����Ӷ���,��������ͷŶ���.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // ����ָ�����ݿ������ַ������ط���.
            FillDataset(connection, commandType, commandText, dataSet, tableNames);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ��������ݼ�.ָ���������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    public static void FillDataset(string connectionString, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        // �����������ݿ����Ӷ���,��������ͷŶ���.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // ����ָ�����ݿ������ַ������ط���.
            FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ������ַ���������,ӳ�����ݱ��������ݼ�,ָ���洢���̲���ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  FillDataset(connString, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, 24);
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>    
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    public static void FillDataset(string connectionString, string spName,
        DataSet dataSet, string[] tableNames,
        params object[] parameterValues)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        // �����������ݿ����Ӷ���,��������ͷŶ���.
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            // ����ָ�����ݿ������ַ������ط���.
            FillDataset(connection, spName, dataSet, tableNames, parameterValues);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ��������ݼ�.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>    
    public static void FillDataset(SqlConnection connection, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames)
    {
        FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ��������ݼ�,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    public static void FillDataset(SqlConnection connection, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params SqlParameter[] commandParameters)
    {
        FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����Ӷ��������,ӳ�����ݱ��������ݼ�,ָ���洢���̲���ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  FillDataset(conn, "GetOrders", ds, new string[] {"orders"}, 24, 36);
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    public static void FillDataset(SqlConnection connection, string spName,
        DataSet dataSet, string[] tableNames,
        params object[] parameterValues)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        }
        else
        {
            // û�в���ֵ
            FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,ӳ�����ݱ��������ݼ�.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    public static void FillDataset(SqlTransaction transaction, CommandType commandType,
        string commandText,
        DataSet dataSet, string[] tableNames)
    {
        FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,ӳ�����ݱ��������ݼ�,ָ������.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    public static void FillDataset(SqlTransaction transaction, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params SqlParameter[] commandParameters)
    {
        FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
    }

    /// <summary>
    /// ִ��ָ�����ݿ����������,ӳ�����ݱ��������ݼ�,ָ���洢���̲���ֵ.
    /// </summary>
    /// <remarks>
    /// �˷������ṩ���ʴ洢������������ͷ���ֵ����.
    /// 
    /// ʾ��:  
    ///  FillDataset(trans, "GetOrders", ds, new string[]{"orders"}, 24, 36);
    /// </remarks>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    /// <param name="parameterValues">������洢������������Ķ�������</param>
    public static void FillDataset(SqlTransaction transaction, string spName,
        DataSet dataSet, string[] tableNames,
        params object[] parameterValues)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ����в���ֵ
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);
            // ���洢���̲�����ֵ
            AssignParameterValues(commandParameters, parameterValues);
            // �������ط���
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
        }
        else
        {
            // û�в���ֵ
            FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
        }
    }

    /// <summary>
    /// [˽�з���][�ڲ�����]ִ��ָ�����ݿ����Ӷ���/���������,ӳ�����ݱ��������ݼ�,DataSet/TableNames/SqlParameters.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="transaction">һ����Ч����������</param>
    /// <param name="commandType">�������� (�洢����,�����ı�������)</param>
    /// <param name="commandText">�洢�������ƻ�T-SQL���</param>
    /// <param name="dataSet">Ҫ���������DataSetʵ��</param>
    /// <param name="tableNames">��ӳ������ݱ�����
    /// �û�����ı��� (������ʵ�ʵı���.)
    /// </param>
    /// <param name="commandParameters">����������SqlParamter��������</param>
    private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType,
        string commandText, DataSet dataSet, string[] tableNames,
        params SqlParameter[] commandParameters)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (dataSet == null) throw new ArgumentNullException("dataSet");
        // ����SqlCommand����,������Ԥ����
        SqlCommand command = new SqlCommand();
        bool mustCloseConnection = false;
        PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

        // ִ������
        using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
        {

            // ׷�ӱ�ӳ��
            if (tableNames != null && tableNames.Length > 0)
            {
                string tableName = "Table";
                for (int index = 0; index < tableNames.Length; index++)
                {
                    if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                    dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                    tableName += (index + 1).ToString();
                }
            }

            // ������ݼ�ʹ��Ĭ�ϱ�����
            dataAdapter.Fill(dataSet);
            // �������,�Ա��ٴ�ʹ��.
            command.Parameters.Clear();
        }
        if (mustCloseConnection)
            connection.Close();
    }
    #endregion

    #region UpdateDataset �������ݼ�
    /// <summary>
    /// ִ�����ݼ����µ����ݿ�,ָ��inserted, updated, or deleted����.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order");
    /// </remarks>
    /// <param name="insertCommand">[׷�Ӽ�¼]һ����Ч��T-SQL����洢����</param>
    /// <param name="deleteCommand">[ɾ����¼]һ����Ч��T-SQL����洢����</param>
    /// <param name="updateCommand">[���¼�¼]һ����Ч��T-SQL����洢����</param>
    /// <param name="dataSet">Ҫ���µ����ݿ��DataSet</param>
    /// <param name="tableName">Ҫ���µ����ݿ��DataTable</param>
    public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
    {
        if (insertCommand == null) throw new ArgumentNullException("insertCommand");
        if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
        if (updateCommand == null) throw new ArgumentNullException("updateCommand");
        if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");
        // ����SqlDataAdapter,��������ɺ��ͷ�.
        using (SqlDataAdapter dataAdapter = new SqlDataAdapter())
        {
            // ������������������
            dataAdapter.UpdateCommand = updateCommand;
            dataAdapter.InsertCommand = insertCommand;
            dataAdapter.DeleteCommand = deleteCommand;
            // �������ݼ��ı䵽���ݿ�
            dataAdapter.Update(dataSet, tableName);
            // �ύ���иı䵽���ݼ�.
            dataSet.AcceptChanges();
        }
    }
    #endregion

    #region CreateCommand ����һ��SqlCommand����
    /// <summary>
    /// ����SqlCommand����,ָ�����ݿ����Ӷ���,�洢�������Ͳ���.
    /// </summary>
    /// <remarks>
    /// ʾ��:  
    ///  SqlCommand command = CreateCommand(conn, "AddCustomer", "CustomerID", "CustomerName");
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="sourceColumns">Դ�������������</param>
    /// <returns>����SqlCommand����</returns>
    public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ��������
        SqlCommand cmd = new SqlCommand(spName, connection);
        cmd.CommandType = CommandType.StoredProcedure;
        // ����в���ֵ
        if ((sourceColumns != null) && (sourceColumns.Length > 0))
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);
            // ��Դ����е�ӳ�䵽DataSet������.
            for (int index = 0; index < sourceColumns.Length; index++)
                commandParameters[index].SourceColumn = sourceColumns[index];
            // Attach the discovered parameters to the SqlCommand object
            AttachParameters(cmd, commandParameters);
        }
        return cmd;
    }
    #endregion

    #region ExecuteNonQueryTypedParams ���ͻ�����(DataRow)
    /// <summary>
    /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQueryTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQueryTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,������Ӱ�������.
    /// </summary>
    /// <param name="transaction">һ����Ч���������� object</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����Ӱ�������</returns>
    public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // Sf the row has values, the store procedure parameters must be initialized
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion

    #region ExecuteDatasetTypedParams ���ͻ�����(DataRow)
    /// <summary>
    /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����һ�������������DataSet.</returns>
    public static DataSet ExecuteDatasetTypedParams(string connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        //���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����һ�������������DataSet.</returns>
    /// 
    public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataSet.
    /// </summary>
    /// <param name="transaction">һ����Ч���������� object</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����һ�������������DataSet.</returns>
    public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion

    #region ExecuteReaderTypedParams ���ͻ�����(DataRow)
    /// <summary>
    /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����DataReader.
    /// </summary>
    /// <param name="transaction">һ����Ч���������� object</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ذ����������SqlDataReader</returns>
    public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion

    #region ExecuteScalarTypedParams ���ͻ�����(DataRow)
    /// <summary>
    /// ִ��ָ���������ݿ������ַ����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalarTypedParams(String connectionString, String spName, DataRow dataRow)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalarTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,���ؽ�����еĵ�һ�е�һ��.
    /// </summary>
    /// <param name="transaction">һ����Ч���������� object</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
    public static object ExecuteScalarTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion

    #region ExecuteXmlReaderTypedParams ���ͻ�����(DataRow)
    /// <summary>
    /// ִ��ָ���������ݿ����Ӷ���Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����XmlReader���͵Ľ����.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, String spName, DataRow dataRow)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }
    }

    /// <summary>
    /// ִ��ָ���������ݿ�����Ĵ洢����,ʹ��DataRow��Ϊ����ֵ,����XmlReader���͵Ľ����.
    /// </summary>
    /// <param name="transaction">һ����Ч���������� object</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="dataRow">ʹ��DataRow��Ϊ����ֵ</param>
    /// <returns>����XmlReader���������.</returns>
    public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, String spName, DataRow dataRow)
    {
        if (transaction == null) throw new ArgumentNullException("transaction");
        if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        // ���row��ֵ,�洢���̱����ʼ��.
        if (dataRow != null && dataRow.ItemArray.Length > 0)
        {
            // �ӻ����м��ش洢���̲���,��������в�����������ݿ��м���������Ϣ�����ص�������. ()
            SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

            // �������ֵ
            AssignParameterValues(commandParameters, dataRow);

            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
        }
        else
        {
            return SqlHelper.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
    #endregion

    
}

/// <summary>
/// SqlHelperParameterCache�ṩ����洢���̲���,���ܹ�������ʱ�Ӵ洢������̽������.
/// </summary>
public sealed class SqlHelperParameterCache
{
    #region ˽�з���,�ֶ�,���캯��
    // ˽�й��캯��,��ֹ�౻ʵ����.
    private SqlHelperParameterCache() { }
    // �������Ҫע��
    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
    /// <summary>
    /// ̽������ʱ�Ĵ洢����,����SqlParameter��������.
    /// ��ʼ������ֵΪ DBNull.Value.
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ�����</param>
    /// <param name="spName">�洢��������</param>
    /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
    /// <returns>����SqlParameter��������</returns>
    private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        SqlCommand cmd = new SqlCommand(spName, connection);
        cmd.CommandType = CommandType.StoredProcedure;
        connection.Open();
        // ����cmdָ���Ĵ洢���̵Ĳ�����Ϣ,����䵽cmd��Parameters��������.
        SqlCommandBuilder.DeriveParameters(cmd);
        connection.Close();
        // �������������ֵ����,���������е�ÿһ������ɾ��.
        if (!includeReturnValueParameter)
        {
            cmd.Parameters.RemoveAt(0);
        }

        // ������������
        SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
        // ��cmd��Parameters���������Ƶ�discoveredParameters����.
        cmd.Parameters.CopyTo(discoveredParameters, 0);
        // ��ʼ������ֵΪ DBNull.Value.
        foreach (SqlParameter discoveredParameter in discoveredParameters)
        {
            discoveredParameter.Value = DBNull.Value;
        }
        return discoveredParameters;
    }

    /// <summary>
    /// SqlParameter�����������㿽��.
    /// </summary>
    /// <param name="originalParameters">ԭʼ��������</param>
    /// <returns>����һ��ͬ���Ĳ�������</returns>
    private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
    {
        SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
        for (int i = 0, j = originalParameters.Length; i < j; i++)
        {
            clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
        }
        return clonedParameters;
    }
    #endregion ˽�з���,�ֶ�,���캯������

    #region ���淽��
    /// <summary>
    /// ׷�Ӳ������鵽����.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
    /// <param name="commandText">�洢��������SQL���</param>
    /// <param name="commandParameters">Ҫ����Ĳ�������</param>
    public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
        string hashKey = connectionString + ":" + commandText;
        paramCache[hashKey] = commandParameters;
    }

    /// <summary>
    /// �ӻ����л�ȡ��������.
    /// </summary>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ�</param>
    /// <param name="commandText">�洢��������SQL���</param>
    /// <returns>��������</returns>
    public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
        string hashKey = connectionString + ":" + commandText;
        SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
        if (cachedParameters == null)
        {
            return null;
        }
        else
        {
            return CloneParameters(cachedParameters);
        }
    }
    #endregion ���淽������

    #region ����ָ���Ĵ洢���̵Ĳ�����
    /// <summary>
    /// ����ָ���Ĵ洢���̵Ĳ�����
    /// </summary>
    /// <remarks>
    /// �����������ѯ���ݿ�,������Ϣ�洢������.
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ�</param>
    /// <param name="spName">�洢������</param>
    /// <returns>����SqlParameter��������</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }

    /// <summary>
    /// ����ָ���Ĵ洢���̵Ĳ�����
    /// </summary>
    /// <remarks>
    /// �����������ѯ���ݿ�,������Ϣ�洢������.
    /// </remarks>
    /// <param name="connectionString">һ����Ч�����ݿ������ַ�.</param>
    /// <param name="spName">�洢������</param>
    /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
    /// <returns>����SqlParameter��������</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
        }
    }

    /// <summary>
    /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���).
    /// </summary>
    /// <remarks>
    /// �����������ѯ���ݿ�,������Ϣ�洢������.
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ������ַ�</param>
    /// <param name="spName">�洢������</param>
    /// <returns>����SqlParameter��������</returns>
    internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
    {
        return GetSpParameterSet(connection, spName, false);
    }

    /// <summary>
    /// [�ڲ�]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
    /// </summary>
    /// <remarks>
    /// �����������ѯ���ݿ�,������Ϣ�洢������.
    /// </remarks>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="includeReturnValueParameter">
    /// �Ƿ��������ֵ����
    /// </param>
    /// <returns>����SqlParameter��������</returns>
    internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
        {
            return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
        }
    }

    /// <summary>
    /// [˽��]����ָ���Ĵ洢���̵Ĳ�����(ʹ�����Ӷ���)
    /// </summary>
    /// <param name="connection">һ����Ч�����ݿ����Ӷ���</param>
    /// <param name="spName">�洢������</param>
    /// <param name="includeReturnValueParameter">�Ƿ��������ֵ����</param>
    /// <returns>����SqlParameter��������</returns>
    private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
    {
        if (connection == null) throw new ArgumentNullException("connection");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");
        string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
        SqlParameter[] cachedParameters;

        cachedParameters = paramCache[hashKey] as SqlParameter[];
        if (cachedParameters == null)
        {
            SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
            paramCache[hashKey] = spParameters;
            cachedParameters = spParameters;
        }

        return CloneParameters(cachedParameters);
    }

    #endregion ��������������
}

