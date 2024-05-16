using Dapper;
using DefaultNamespace;
using Npgsql;

namespace infrastructure.Repositories;

public class StatusRepository
{
    public readonly NpgsqlDataSource _DataSource;


    public StatusRepository(NpgsqlDataSource dataSource)
    {
        _DataSource = dataSource;
    }
    
    
    public StatusModel CreateStatusEntry(string log, DateTime date)
    {
        var sql = "INSERT INTO ph.status(log, date) VALUES (@log, @date);";

        using (var conn = _DataSource.OpenConnection())
        {
            return conn.QueryFirst(sql, new { log = log, date = date });
        }
    }
    
    public StatusModel GetLatestEntry()
    {
        
        var sql = $@"SELECT * FROM ph.status ORDER BY date DESC LIMIT 1;";
        
    
        using (var conn = _DataSource.OpenConnection())
        {
            //return conn.QueryFirstOrDefault<StatusModel>(sql, new StatusModel());
            return new StatusModel { Date = DateTime.Today, Log = "hello" };
        }
    }
    
    
    
    
    
}