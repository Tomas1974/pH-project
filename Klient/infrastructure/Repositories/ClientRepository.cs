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
        var sql =
            "SELECT ph.client.client_id, client_name, max_value, min_value FROM ph.client INNER JOIN ph.client_user ON ph.client.client_id = ph.client_user.client_id WHERE email = @user_id;";

        using (var conn = _DataSource.OpenConnection())
        {
            return conn.Query<ClientModel>(sql, new { user_id = user_id });
        }
    }

    public ClientModel CreateClient(ClientModel clientModel, string email)
    {
        var updateSql = 
            "UPDATE ph.client SET client_name=@client_name, max_value=@max_value, min_value=@min_value WHERE client_id=@client_id;";

        var insertSql = 
            "INSERT INTO ph.client_user(client_id, email) VALUES (@client_id, @Email);";

        var selectSql = 
            "SELECT client_id, client_name, max_value, min_value FROM ph.client WHERE client_id=@client_id;";

        using (var conn = _DataSource.OpenConnection())
        {
            using (var transaction = conn.BeginTransaction())
            {
                conn.Execute(updateSql, 
                    new
                    {
                        client_id = clientModel.client_id,
                        client_name = clientModel.client_name,
                        max_value = clientModel.max_value,
                        min_value = clientModel.min_value
                    }, transaction);

                conn.Execute(insertSql, 
                    new
                    {
                        client_id = clientModel.client_id,
                        Email = email
                    }, transaction);

                var updatedClient = conn.QueryFirst<ClientModel>(selectSql, 
                    new
                    {
                        client_id = clientModel.client_id
                    }, transaction);

                transaction.Commit();

                return updatedClient;
            }
        }
    }

    
    
    
    
    public void createClientUser(string clientId, string email)
    {
        var sql = @"INSERT INTO ph.client_user(client_id, email) VALUES (@ClientId, @Email);";

      
            using (var conn = _DataSource.OpenConnection())
            {
                conn.Execute(sql, new { ClientId = clientId, Email = email });
            }
       
    }
    
    

    public bool ClientAlreadyUsed(string client_id)
    {
        var sql = "SELECT COUNT(*) FROM ph.client_user WHERE client_id = @client_id";

        using (var conn = _DataSource.OpenConnection())
        {
            var count = conn.QueryFirstOrDefault<int>(sql, new { client_id = client_id });
            return count > 0;
        }
    }
    
    
    
    public bool ClientExits(string client_id)
    {
        var sql = "SELECT COUNT(*) FROM ph.client WHERE client_id = @client_id";

        using (var conn = _DataSource.OpenConnection())
        {
            var count = conn.QueryFirstOrDefault<int>(sql, new { client_id = client_id });
            return count > 0;
        }
    }
    
    
    
    
    
    

    public void deleteClient(string client_Id)
    {
        var sql = "UPDATE ph.client SET client_name=null, max_value=null, min_value=null WHERE client_id=@client_id; DELETE FROM ph.client_user WHERE client_id = @client_id";
        using (var conn = _DataSource.OpenConnection())
        {
            conn.Execute(sql, new { client_Id = client_Id });
        }
    }
}