using System.Collections;
using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure;

public class DataRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public DataRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    
    
    public IEnumerable<DataModel> FindData(string client)
    {
        string sql = @"SELECT data, time FROM ph.data WHERE client_id = @Client_id;";
    
        using (var conn = _dataSource.OpenConnection())
        {
                return conn.Query<DataModel>(sql, new { Client_id = client });
        }
    }
    
}