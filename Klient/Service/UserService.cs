using infrastructure;
using infrastructure.DataModels;

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
        return _userRepository.CreateUser(userModel);

    }
    
    


}