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
                VALUES (@Email,@Name, @Hash, @Salt, @Address, @ZipCode, @Cvr) 
                RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            
            return conn.QueryFirst<UserModel>(sql, new
            {
                Email=saveToDatabase.email.ToLower(),
                Name = saveToDatabase.name,       
                Hash = saveToDatabase.hash,
                Salt = saveToDatabase.salt,
                Address = saveToDatabase.address,
                ZipCode = saveToDatabase.zip_code, 
                Cvr = saveToDatabase.cvr
            });
        }
    }
    
    public UserSaveToDatabaseModel FindUser(String Email)
    {
        
        var sql = $@"SELECT * FROM ph.users WHERE email = @Email;";
    
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirstOrDefault<UserSaveToDatabaseModel>(sql, new { Email=Email.ToLower() }); //Denne QueryFirstOrDefault acceptere null værdier.
        }
    }
    
    
    public void DeleteUser(string email)
    {
        var sql = @"DELETE FROM ph.users WHERE email = @Email RETURNING *;";

        using (var conn = _dataSource.OpenConnection())
        {
            conn.QueryFirst<CheckLoginModel>(sql, new { Email=email });
        }
    }
}