using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace CustomerPortalWebApi.Interface
{
    public interface ICustomerPortalHelper : IDisposable
    {
        DbConnection GetConnection();

        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        DataTable GetDataTable(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        int ExecuteTrans(IDbTransaction transaction,string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        List<T> Count<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T UpdateTrans<T>(IDbTransaction transaction,string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T InsertTrans<T>(IDbTransaction transactionstring,string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}