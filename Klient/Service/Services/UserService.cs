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
                email = userModel.email,
                name = userModel.name, 
                hash =  hashPassword,
                salt = salt,
                address =userModel.address,
                zip_code =userModel.zip_code,
                cvr =userModel.cvr
                    
                };
            
      
      
            return _userRepository.CreateUser(saveToDatabase);

    }

    public bool loginUser(LoginModel loginModel)
    {
        PasswordHashService passwordHashService = new PasswordHashService();

        CheckLoginModel checkLoginModel = _userRepository.FindUser(loginModel.email);
        
        string hashPassword = passwordHashService.HashPassword(loginModel.password, checkLoginModel.salt);
        
        if (hashPassword.Equals(checkLoginModel.hash))
            return true;
        else
            return false;

    }


}