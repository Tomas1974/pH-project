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


    public void updateUser(UserSaveToDatabaseModel saveToDatabase, string oldEmail)
{
    using (var conn = _dataSource.OpenConnection())
    {
        using (var transaction = conn.BeginTransaction())
        {
            
            try
            {
                // var sql = @"SELECT client_id FROM ph.client_user WHERE email = @Email;";
                // var liste = conn.Query<string>(sql, new { Email = oldEmail }).ToList();
                //
                //
                //
                //
                // foreach (var clientId in liste)
                // {
                //     var updateClientUser = @"UPDATE ph.client_user SET email = @NewEmail WHERE client_id = @ClientId;";
                //     conn.Execute(updateClientUser, new { NewEmail = saveToDatabase.email, ClientId = clientId }, transaction);
                // }
                
                
                //
                // var updateUser = @"UPDATE ph.users SET email = @Email, name = @Name, hash = @Hash, salt = @Salt, address = @Address, 
                //                    street_number = @StreetNumber, zip_code = @ZipCode, cvr = @Cvr WHERE email = @OldEmail;
                //                     ";
                // conn.Execute(updateUser, 
                //     new
                //     {
                //         Email = saveToDatabase.email,
                //         Name = saveToDatabase.name,
                //         Hash = saveToDatabase.hash,
                //         Salt = saveToDatabase.salt,
                //         Address = saveToDatabase.address,
                //         StreetNumber = saveToDatabase.street_number,
                //         ZipCode = saveToDatabase.zip_code,
                //         Cvr = saveToDatabase.cvr,
                //         OldEmail = oldEmail
                //     }, transaction);


               
                
                
                var updateUser = @"UPDATE ph.users SET email = @Email, name = @Name, hash = @Hash, salt = @Salt, address = @Address, 
                                   street_number = @StreetNumber, zip_code = @ZipCode, cvr = @Cvr
                                   INNER JOIN ph.users on ph.users.email=ph.client_user.email  
                    
                                WHERE email = @OldEmail; ";
                conn.Execute(updateUser, 
                    new
                    {
                        Email = saveToDatabase.email,
                        Name = saveToDatabase.name,
                        Hash = saveToDatabase.hash,
                        Salt = saveToDatabase.salt,
                        Address = saveToDatabase.address,
                        StreetNumber = saveToDatabase.street_number,
                        ZipCode = saveToDatabase.zip_code,
                        Cvr = saveToDatabase.cvr,
                        OldEmail = oldEmail
                    }, transaction);



                
                
                
                
                
                
                
                
                
                
                
                
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }
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

                 var updatesql =
                     "UPDATE ph.client SET client_name=null, max_value=null, min_value=null WHERE client_id=@clientId;";
                  conn.Execute(updatesql,new { clientId = clientId });

             }
             
             
              var sql1 = @"DELETE FROM ph.users WHERE email = @Email;";
            
            
            conn.Execute(sql1, new { Email=email });
            
            
            transaction.Commit();
            
        }
        
    }
}