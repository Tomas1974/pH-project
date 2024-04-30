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
            @" INSERT INTO ph.users (name, password, address, zip_code, cvr) VALUES (@name, @password, @address, @zip_code, @cvr) RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<UserModel>(sql, new { name=userModel.username, password=userModel.password, address=userModel.address, zip_code=userModel.zipcode, cvr=userModel.cvr });
        }
    }
    
    
    
}