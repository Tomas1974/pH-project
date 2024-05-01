using infrastructure;
using infrastructure.DataModels;
using service.Services;

namespace Service1;

public class UserService
{
    
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    


    public UserModel CreateUser(UserModel userModel)
    {

        PasswordHashService passwordHashService = new PasswordHashService();
        string salt = passwordHashService.GenerateSalt();
        string hashPassword = passwordHashService.HashPassword(userModel.password, salt);
        
      
            
            UserSaveToDatabaseModel saveToDatabase= new UserSaveToDatabaseModel
            
                {
                username = userModel.username, 
                hash =  hashPassword,
                salt = salt,
                address =userModel.address,
                zipcode =userModel.zipcode,
                cvr =userModel.cvr
                    
                };
            
      
      
            return _userRepository.CreateUser(saveToDatabase);

    }
    
    


}