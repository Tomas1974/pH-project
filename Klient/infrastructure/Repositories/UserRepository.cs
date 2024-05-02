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
    
    public UserModel CreateUser(UserSaveToDatabaseModel saveToDatabase)
    {
        var sql = @"INSERT INTO ph.users (name, hash, salt, address, zip_code, cvr) 
                VALUES (@Name, @Hash, @Salt, @Address, @ZipCode, @Cvr) 
                RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            
            return conn.QueryFirst<UserModel>(sql, new
            {
                Name = saveToDatabase.username,       
                Hash = saveToDatabase.hash,
                Salt = saveToDatabase.salt,
                Address = saveToDatabase.address,
                ZipCode = saveToDatabase.zipcode, 
                Cvr = saveToDatabase.cvr
            });
        }
    }
    
    
    
}