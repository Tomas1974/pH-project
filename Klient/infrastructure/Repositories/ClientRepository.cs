using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure.Repositories;

public class ClientRepository
{
    public readonly NpgsqlDataSource _DataSource;

    public ClientRepository(NpgsqlDataSource dataSource)
    {
        _DataSource = dataSource;
    }

    public ClientModel CreateClient(ClientModel clientModel)
    {
        var sql = "INSERT INTO ph.client(client_id, client_name, max_value, min_value) VALUES (@client_id, @client_name, @max_value, @min_value);";

        using (var conn = _DataSource.OpenConnection())
        {
           return conn.QueryFirst(sql,
                new { client_id = clientModel.client_id, client_name = clientModel.client_name, max_value = clientModel.max_value, min_value = clientModel.min_value });
        }
    } 


}

