using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure.Repositories;

public class StatusRepository
{
    public readonly NpgsqlDataSource _DataSource;
    
    public StatusRepository(NpgsqlDataSource dataSource)
    {
        _DataSource = dataSource;
    }
    
    //Creates a new entry in the ph.status table.
    public StatusModel CreateStatusEntry(string log, DateTime date)
    {
        var sql = "INSERT INTO ph.status(log, date) VALUES (@log, @date);";

        using (var conn = _DataSource.OpenConnection())
        {
            return conn.QueryFirst(sql, new { log = log, date = date });
        }
    }
    
    
    //Returns latest entry in the ph.status table.
    public StatusModel GetLatestEntry()
    {
        
        var sql = $@"SELECT * FROM ph.status ORDER BY date DESC LIMIT 10;";
        
    
        using (var conn = _DataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<StatusModel>(sql, new StatusModel());
        }
    }
}