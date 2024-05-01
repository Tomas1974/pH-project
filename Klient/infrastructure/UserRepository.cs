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
        var sql = @"INSERT INTO ph.users (name, password, address, zip_code, cvr) 
                VALUES (@Name, @Password, @Address, @ZipCode, @Cvr) 
                RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            
            return conn.QueryFirst<UserModel>(sql, new
            {
                Name = userModel.username,       
                Password = userModel.password,
                Address = userModel.address,
                ZipCode = userModel.zipcode, 
                Cvr = userModel.cvr
            });
        }
    }
    
    
    
}