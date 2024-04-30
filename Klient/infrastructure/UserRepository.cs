using Dapper;
using infrastructure.DataModels;
using Npgsql;

namespace infrastructure;

public class UserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    
    public UserModel CreateUser(UserModel userModel)
    {
        var sql =
            @" INSERT INTO ph.users (username, password, address, zipcode, cvr) VALUES (@username, @password, @address, @zipcode, @cvr) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<UserModel>(sql, new { username=userModel.username, password=userModel.password, address=userModel.address, zipcode=userModel.zipcode, cvr=userModel.cvr });
        }
    }
    
    
    
}