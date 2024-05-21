using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service.Services;

namespace api.WS_request.Home_page;

public class LoginUser : BaseEventHandler<loginUserDto>
{
    public readonly UserService _userService;

    public LoginUser(UserService userService)
    {
        _userService = userService;
    }


    public override Task Handle(loginUserDto dto, IWebSocketConnection socket)
    {
        LoginModel loginModel = new LoginModel
        {
            email = dto.email,
            password = dto.password
        };

        string login = _userService.loginUser(loginModel);


        var message = new responseString()
        {
            response = login
        };

        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);

        return Task.CompletedTask;
    }
}

public class loginUserDto : BaseDto
{
    public string email { get; set; }
    public string password { get; set; }
}

public class responseString : BaseDto
{
    public string response { get; set; }
}