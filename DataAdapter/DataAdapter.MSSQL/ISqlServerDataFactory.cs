﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAdapter.MSSQL
{
    public interface ISqlServerDataFactory
    {
        /// <summary>
        /// 删除数据库数据数据
        /// </summary>
        /// <param name="delete"></param>
        /// <returns></returns>
        int Remove(Delete delete);
        /// <summary>
        /// 向数据库插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="insert"></param>
        /// <returns></returns>
        int Save<T>(Insert<T> insert);
        /// <summary>
        /// 更新数据库数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="update"></param>
        /// <returns></returns>
        int Save<T>(Update<T> update);
        /// <summary>
        /// 一般检索数据库数据,返回DataTable
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DataTable Query(Query query);
        /// <summary>
        /// 复杂检索数据库数据，返回DataTable
        /// </summary>
        /// <param name="complexQuery"></param>
        /// <returns></returns>
        DataTable Query(ComplexQuery complexQuery);
        /// <summary>
        /// 一般检索数据库数据，返回泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(Query query);
        /// <summary>
        /// 复杂检索数据库数据，返回泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="complexQuery"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(ComplexQuery complexQuery);
        /// <summary>
        /// 根据连接字符串和参数集合查询数据
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable Query(string queryString, params SqlParameter[] parameters);
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        int ExecuteSQL(string sqlString);
        /// <summary>
        /// 执行带参数SQL语句
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        int ExecuteSQL(string sqlString, params SqlParameter[] parameters);
        /// <summary>
        /// 执行SQL语句或者存储过程
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        int ExecuteSQL(string sqlString, bool isStoredProcedure, params SqlParameter[] parameters);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="sourceTable"></param>
        /// <returns></returns>
        int Save(string tableName, DataTable sourceTable);
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="tableName">目标数据库表名</param>
        /// <param name="sourceTable">源数据表</param>
        /// <param name="keyColumnName">更新标准字段</param>
        /// <returns>成功返回受影响的行数，失败返回-1</returns>
        int Update(string tableName, DataTable sourceTable, string[] keyColumnName);
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="tableName">目标数据库表名</param>
        /// <param name="sourceTable">源数据表</param>
        /// <param name="excludeColumnName">需排除字段</param>
        /// <returns>返回受影响的行数</returns>
        int Insert(string tableName, DataTable sourceTable, params string[] excludeColumnName);
    }
}
