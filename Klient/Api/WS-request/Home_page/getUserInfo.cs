using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service.Services;

namespace api.WS_request.Home_page;

public class getUserInfo : BaseEventHandler<getUserInfoDto>
{
    public readonly UserService _userService;

    public getUserInfo(UserService userService)
    {
        _userService = userService;
    }


    public override Task Handle(getUserInfoDto dto, IWebSocketConnection socket)
    {
        UserModel userModel = _userService.getUserInfo(dto.email);


        var message = new userModelDto()
        {
            email = userModel.email,
            name = userModel.name,
            address = userModel.address,
            street_number = userModel.street_number,
            zip_code = userModel.zip_code,
            cvr = userModel.cvr
        };


        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);

        return Task.CompletedTask;
    }
}

public class getUserInfoDto : BaseDto
{
    public string email { get; set; }
}

public class userModelDto : BaseDto
{
    public string email { get; set; }
    public string name { get; set; }

    public string address { get; set; }
    public int zip_code { get; set; }
    public string street_number { get; set; }
    public int? cvr { get; set; }
}