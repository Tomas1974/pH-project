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
        var sql = @"INSERT INTO ph.users (email,name, hash, salt, address, street_number, zip_code, cvr) 
                VALUES (@Email,@Name, @Hash, @Salt, @Address, @street_number,@ZipCode, @Cvr) 
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
                Street_number=saveToDatabase.street_number,
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
       

        using (var conn = _dataSource.OpenConnection())
        {
            
            var transaction = conn.BeginTransaction();
            
             var sql = $@"SELECT client_id FROM ph.client_user WHERE email = @Email;";
                
             var liste = conn.Query<string>(sql, new { Email = email }).ToList();

             foreach (var clientId in liste)
             {
                 var deleteSql = @"DELETE FROM ph.data WHERE client_id = @ClientId;";
                 conn.Execute(deleteSql, new { ClientId = clientId });
                
                 
                 var deleteClient = @"Delete from ph.client_user  WHERE email = @Email;";
                 conn.Execute(deleteClient, new {  Email = email });
                 
                 
                 
                 
             }
             
             
             
            
             // foreach (var clientId in liste)
             // {
             //     var deleteSql = @"DELETE FROM ph.data WHERE client_id = @ClientId;";
             //     conn.Execute(deleteSql, new { ClientId = clientId }, transaction);
             // }
             //
             
             
            //   var sql1 = @"DELETE FROM ph.users WHERE email = @Email RETURNING *;";
            //
            //
            // conn.QueryFirst<CheckLoginModel>(sql1, new { Email=email });
            
            
            transaction.Commit();
            
        }
        
    }
}