using System.ComponentModel.DataAnnotations;
using infrastructure;
using infrastructure.DataModels;
using service.Services;

namespace Service1;

public class UserService
{
    public string loginEmail = "";
    
    
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


             if (checkLoginModel == null) //Checker om emailen er brugt før
             {
                 _userRepository.CreateUser(saveToDatabase);
                 loginEmail = userModel.email; //gemmer hvem der er login. Hvis browseren bliver genindlæst kan man slå op, hvem der er logget ind.
             }    
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

            if (checkLoginModel != null)
            {
                string hashPassword = passwordHashService.HashPassword(loginModel.password, checkLoginModel.salt);

                if (hashPassword.Equals(checkLoginModel.hash))
                {
                    loginEmail = loginModel.email;
                    return "Success";
                    
                }
                else
                    return "Wrong username or password";
            }
           
            else
                return "Wrong username or password";
        }
        catch 
        {
            
            throw new ValidationException("An error occured login");
        }
        

    }


}