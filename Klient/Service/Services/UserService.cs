using System.ComponentModel.DataAnnotations;
using infrastructure;
using infrastructure.DataModels;
using infrastructure.Repositories;

namespace Service.Services;

public class UserService
{
    public string loginEmail = "";


    private readonly UserRepository _userRepository;
    private readonly ClientRepository _clientRepository;
    
    public UserService(UserRepository userRepository, ClientRepository clientRepository)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
    }


    public string CreateOrUpdateUser(UserModel userModel, string type, string oldEmail)
    {
        try
        {
            UserSaveToDatabaseModel saveToDatabase= makeUserSaveToDatabaseModel(userModel);
            
            UserSaveToDatabaseModel checkLoginModel = _userRepository.FindUser(userModel.email);


            if (type=="newUser") //ny bruger
                if (checkLoginModel == null) //checker om email har været brugt før
                _userRepository.CreateUser(saveToDatabase);
                
                else
                    return "Email already used";
           
            
            else //Opdatering af eksisterende bruger
            {
                if (oldEmail==saveToDatabase.email) //ny og gammel email er uforandret
                    _userRepository.updateUser(saveToDatabase, oldEmail);
                
                else if (checkLoginModel != null) //email er brugt før og den er ikke uforandret
                    return "Email already used";
                
                
                else //ny og gammel email er forskellige
                {
                    
                    _userRepository.CreateUser(saveToDatabase); //Her gemmes brugeren med den nye email.
                    
                    List<string> liste= _userRepository.findClients(oldEmail); //først en liste over clienter
                    
                    foreach (var clientId in liste)
                    {
                        _clientRepository.createClientUser(clientId, saveToDatabase.email); //Så gemmes clienterne med den nye email
                    }

                    _userRepository.DeleteUser(oldEmail); //Her slettes den gamle bruger og hans clienter
                }
            }
                    
                
        loginEmail = userModel.email;
        return "Success";
        
        }
        catch
        {
            Console.WriteLine("Fejl på gemning");
            throw new ValidationException("An error occured creating user");
        }
    }

    private UserSaveToDatabaseModel makeUserSaveToDatabaseModel(UserModel userModel)
    {
        PasswordHashService passwordHashService = new PasswordHashService();
        string salt = passwordHashService.GenerateSalt();
        string hashPassword = passwordHashService.HashPassword(userModel.password, salt);
        
         return new UserSaveToDatabaseModel

        {
            email = userModel.email,
            name = userModel.name,
            hash = hashPassword,
            salt = salt,
            address = userModel.address,
            street_number = userModel.street_number,
            zip_code = userModel.zip_code,
            cvr = userModel.cvr
        };
        
    }


    public string loginUser(LoginModel loginModel)
    {
        try
        {
            PasswordHashService passwordHashService = new PasswordHashService();

            UserSaveToDatabaseModel checkLoginModel = _userRepository.FindUser(loginModel.email);

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

    public void deleteUser()
    {
        try
        {
            _userRepository.DeleteUser(loginEmail);
            loginEmail = "";
        }
        catch
        {
            throw new ValidationException("Error in deleting user");
        }
    }


    public UserModel getUserInfo(string email)
    {
        try
        {
            UserSaveToDatabaseModel userSaveToDatabase = _userRepository.FindUser(email);

            Console.WriteLine("Check Address " + userSaveToDatabase.address);

            UserModel userModel = new UserModel
            {
                email = userSaveToDatabase.email,
                name = userSaveToDatabase.name,
                zip_code = userSaveToDatabase.zip_code,
                address = userSaveToDatabase.address,
                street_number = userSaveToDatabase.street_number,
                password = "",
                cvr = userSaveToDatabase.cvr,
            };

            return userModel;
        }
        catch
        {
            throw new ValidationException("Error getting user");
        }
    }
}