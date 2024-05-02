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
        var sql = @"INSERT INTO ph.users (email,name, hash, salt, address, zip_code, cvr) 
                VALUES (@Email,@Name, @Hash, @Salt, @Address, @Name@ZipCode, @Cvr) 
                RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            
            return conn.QueryFirst<UserModel>(sql, new
            {
                Email=saveToDatabase.email,
                Name = saveToDatabase.username,       
                Hash = saveToDatabase.hash,
                Salt = saveToDatabase.salt,
                Address = saveToDatabase.address,
                ZipCode = saveToDatabase.zipcode, 
                Cvr = saveToDatabase.cvr
            });
        }
    }
    
    // public LoginModel FindUser(int boxId)
    // {
    //     var sql = $@"SELECT * FROM ph.users WHERE box_id = @boxId;";
    //
    //     using (var conn = _dataSource.OpenConnection())
    //     {
    //         return conn.QueryFirst<Box>(sql, new { boxId });
    //     }
    // }
    
}