using System.ComponentModel.DataAnnotations;
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
    


    public string CreateUser(UserModel userModel)
    {
        try
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
            
             CheckLoginModel checkLoginModel = _userRepository.FindUser(userModel.email);
              
             
             if (checkLoginModel == null )
                     _userRepository.CreateUser(saveToDatabase);    
             else
             
                     return "Email already used";
                
            
            return "Success";
        }
        catch 
        {
            Console.WriteLine("Fejl på gemning");
            throw new ValidationException("An error occured creating user");
        }
        
       

    }

    public string loginUser(LoginModel loginModel)
    {
        try
        {
            PasswordHashService passwordHashService = new PasswordHashService();

            CheckLoginModel checkLoginModel = _userRepository.FindUser(loginModel.email);
        
            string hashPassword = passwordHashService.HashPassword(loginModel.password, checkLoginModel.salt);
        
            if (hashPassword.Equals(checkLoginModel.hash))
                return "Success";
            else
                return "Wrong username or password";
        }
        catch 
        {
            
            throw new ValidationException("An error occured login");
        }
        

    }


}