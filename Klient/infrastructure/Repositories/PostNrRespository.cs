using Dapper;
using Npgsql;

namespace infrastructure.Repositories;

public class PostNrRespository
{
    public readonly NpgsqlDataSource _dataSource;
    
    public PostNrRespository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    
    
    public string FindTown(int postNr)
    {
        
        var sql = $@"SELECT Bynavn FROM ph.Postnumre WHERE Postnr = @Postnr;";
    
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<string>(sql, new { Postnr=postNr });
        }
    }
    
    
}