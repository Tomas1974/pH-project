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

    public IEnumerable<ClientModel> GetClients(string user_id)
    {
        var sql = "SELECT ph.client.client_id, client_name, max_value, min_value FROM ph.client INNER JOIN ph.client_user ON ph.client.client_id = ph.client_user.client_id WHERE email = @user_id;";

        using (var conn = _DataSource.OpenConnection())
        {
            return conn.Query<ClientModel>(sql, new { user_id = user_id });
        }
    }
    
    public ClientModel CreateClient(ClientModel clientModel, string email)
    {
        var sql = "INSERT INTO ph.client(client_id, client_name, max_value, min_value) VALUES (@client_id, @client_name, @max_value, @min_value); INSERT INTO ph.client_user(client_id, email) VALUES (@client_id, @email);";

        using (var conn = _DataSource.OpenConnection())
        {
            
           return conn.QueryFirst<ClientModel>(sql,
                new { client_id = clientModel.client_id, client_name = clientModel.client_name, max_value = clientModel.max_value, min_value = clientModel.min_value, email = email });
        }
    }

    public string ClientAlreadyExist(string client_id)
    {
        var sql = "SELECT client_id FROM ph.client_user WHERE client_id = @client_id";

        using (var conn = _DataSource.OpenConnection())
        {
            return conn.QueryFirst<string>(sql, new { client_id = client_id });
        }
    }
}

