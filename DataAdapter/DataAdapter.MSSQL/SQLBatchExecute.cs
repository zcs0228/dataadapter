using DataAdapter.MSSQL.Infrastruction;
using DataAdapter.MSSQL.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataAdapter.MSSQL
{
    public class SQLBatchExecute
    {
        private string _connectionString;

        public SQLBatchExecute()
        {
            _connectionString = ConnectionStringFactory.DataAdapterConnectionString;
        }

        public SQLBatchExecute(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        public int Save(string tableName, DataTable sourceTable)
        {
            int affected = 0;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints, transaction))
                    {
                        bulkCopy.BatchSize = 10;
                        bulkCopy.BulkCopyTimeout = 60;
                        bulkCopy.DestinationTableName = tableName;
                        try
                        {
                            bulkCopy.WriteToServer(sourceTable);
                            transaction.Commit();
                            affected = sourceTable.Rows.Count;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            affected = -1;
                            throw new Exception(ex.Source + ":" + ex.Message);
                        }
                    }
                }
            }

            return affected;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="tableName">目标数据库表名</param>
        /// <param name="sourceTable">数据源表</param>
        /// <param name="keyColumnName">更新标准字段</param>
        /// <returns>成功返回受影响的行数，失败返回-1</returns>
        public int Update(string tableName, DataTable sourceTable, string[] keyColumnName)
        {
            int result;
            string[] columns = TranslateHelper.GetDataTableColumnName(sourceTable, keyColumnName);

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                try
                {
                    foreach (DataRow dr in sourceTable.Rows)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        string updateStr = DataTableTranslateHelper.DataRowToUpdate(tableName, dr, columns, keyColumnName, parameters);
                        ExecuteSQLForTransaction(cmd, updateStr, parameters.ToArray());
                    }
                    transaction.Commit();
                    result = sourceTable.Rows.Count;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Source + ":" + ex.Message);
                }

            }
            return result;
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="tableName">目标数据库表名</param>
        /// <param name="sourceTable">源数据表</param>
        /// <param name="excludeColumnName">需排除字段</param>
        /// <returns>返回受影响的行数</returns>
        public int Insert(string tableName, DataTable sourceTable, params string[] excludeColumnName)
        {
            int result;
            string[] columns = TranslateHelper.GetDataTableColumnName(sourceTable, excludeColumnName);

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                SqlCommand cmd = conn.CreateCommand();
                cmd.Transaction = transaction;

                try
                {
                    foreach (DataRow dr in sourceTable.Rows)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        string insertStr = DataTableTranslateHelper.DataRowToInsert(tableName, dr, columns, parameters);
                        ExecuteSQLForTransaction(cmd, insertStr, parameters.ToArray());
                    }
                    transaction.Commit();
                    result = sourceTable.Rows.Count;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception(ex.Source + ":" + ex.Message);
                }
            }
            return result;
        }

        /// <summary>
        /// 批处理执行函数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sqlString"></param>
        /// <param name="parameters"></param>
        private void ExecuteSQLForTransaction(SqlCommand cmd, string sqlString, params SqlParameter[] parameters)
        {
            cmd.CommandText = sqlString;
            foreach (var item in parameters)
            {
                cmd.Parameters.Add(item);
            }
            cmd.ExecuteNonQuery();
        }
    }
}
