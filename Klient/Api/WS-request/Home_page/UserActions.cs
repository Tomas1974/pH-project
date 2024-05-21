using System.Text.Json;
using Fleck;
using lib;
using Service.Services;

namespace api.WS_request.Home_page;

public class UserActions : BaseEventHandler<LoggedInInfo>
{
    public readonly UserService _userService;

    public UserActions(UserService userService)
    {
        _userService = userService;
    }


    public override Task Handle(LoggedInInfo dto, IWebSocketConnection socket)
    {
        if (dto.getLoginInfo.Contains("logOff"))
        {
            _userService.loginEmail = "";
        }
        else if (dto.getLoginInfo.Contains("delete"))
        {
            _userService.deleteUser();
            Console.WriteLine("delete");
        }


        var message = new SendLoginInfoDto()
        {
            email = _userService.loginEmail
        };

        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);

        return Task.CompletedTask;
    }
}

public class LoggedInInfo : BaseDto
{
    public string getLoginInfo { get; set; }
}

public class SendLoginInfoDto : BaseDto
{
    public string? email { get; set; }
}