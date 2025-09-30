using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;

public class SQLiteHelper
{
    /// <summary>
    /// 数据库连接定义
    /// </summary>
    private SqliteConnection dbConnection;

    /// <summary>
    /// SQL命令定义
    /// </summary>
    private SqliteCommand dbCommand;

    /// <summary>
    /// 数据读取定义
    /// </summary>
    private SqliteDataReader dataReader;

    /// <summary>
    /// 构造函数   
    /// </summary>
    /// <param name="connectionString">数据库连接字符串</param>
    public SQLiteHelper(string connectionString)
    {
        try
        {
            //构造数据库连接
            dbConnection = new SqliteConnection(connectionString);
            //打开数据库
            dbConnection.Open();

        }
        catch (Exception e)
        {
            Debug.Log("Open faild => " +  e.Message);
        }
    }

    /// <summary>
    /// 执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryString;
        dataReader = dbCommand.ExecuteReader();
        return dataReader;
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseConnection()
    {
        //销毁Command
        if (dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;

        //销毁Reader
        if (dataReader != null)
        {
            dataReader.Close();
        }
        dataReader = null;

        //销毁Connection
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
    }

    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader ReadPartTable(string tableName, int Index)
    {
        string queryString = "SELECT * FROM " + tableName + " where  rowid >= " + Index.ToString() + " and " + "rowid < " + (Index + 20).ToString();
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 向指定数据表中插入数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertValues(string tableName, string[] values)
    {
        //获取数据表中字段数目
        int fieldCount = ReadFullTable(tableName).FieldCount;
        //当插入的数据长度不等于字段数目时引发异常
        if (values.Length != fieldCount)
        {
            throw new SqliteException("values.Length!=fieldCount");
        }

        string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; i++)
        {
            queryString += ", " + values[i];
        }
        queryString += " )";
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader ReadTimeValues(string tableName, string values, string values1, string values2)
    {
        ////获取数据表中字段数目
        //int fieldCount = ReadFullTable(tableName).FieldCount;
        ////当插入的数据长度不等于字段数目时引发异常
        //if (values.Length != fieldCount)
        //{
        //    throw new SqliteException("values.Length!=fieldCount");
        //}

        string queryString = "select * from  " + tableName + " where  " + values + " Between '" + values1 + "' and  '" + values2 + "'";
        // string queryString = "select * from Select_6 where All_PeriodTime Between '2022-09-04' and  '2022-09-06'";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名相应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字相应的值</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues)//, string key, string operation, string value)
    {
        //当字段名称和字段数值不正确应时引发异常
        if (colNames.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length");
        }

        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += ", " + colNames[i] + "=" + colValues[i];
        }
   //     queryString += " WHERE " + key + operation + value;
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader UpdateValuesOne(string tableName, string[] colNames, string[] colValues, string x_Index)
    {
        //当字段名称和字段数值不正确应时引发异常
        if (colNames.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length");
        }

        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += ", " + colNames[i] + "=" + colValues[i];
        }
        queryString += " WHERE rowid = " + x_Index;
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名相应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不正确应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += "OR " + colNames[i] + operations[0] + colValues[i];
        }
        queryString += "UPDATE sqlite_sequence SET seq = 0 WHERE name = '" + tableName + "'";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名相应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
    {
        //当字段名称和字段数值不正确应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += " AND " + colNames[i] + operations[i] + colValues[i];
        }
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader DeleteValuesALL(string tableName)
    {
        string queryString = "DELETE FROM " + tableName;
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader UpdataMainKey(string tableName)
    {
        string queryString = "UPDATE sqlite_sequence SET seq = 0 WHERE name = " + "'" + tableName + "'";
        //string queryString = "delete from sqlite_sequence WHERE name = " + "'" + tableName + "'";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.</param>
    /// <param name="items">Items.</param>
    /// <param name="colNames">Col names.</param>
    /// <param name="operations">Operations.</param>
    /// <param name="colValues">Col values.</param>
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        string queryString = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader UpdateTable(string tableName, string[] items, string[] itemValues, string[] colNames, string[] operations, string[] colValues)
    {
        string queryString = "UPDATE " + tableName + " SET " + items[0] + " = " + itemValues[0];
        for (int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i] + " = " + itemValues[i];
        }
        queryString += " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[i] + " ";
        }
        return ExecuteQuery(queryString);
    }

    public SqliteDataReader ClearTable(string tableName)
    {
        string queryString = "DELETE FROM " + tableName;
        return ExecuteQuery(queryString);
    }
}