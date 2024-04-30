using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;
using Service1.Service;

namespace ws;


public class saveUser : BaseEventHandler<UserModelDto>
{
    public readonly UserService _UserService;
    
    public saveUser(UserService userService)
    {
        _UserService = userService;
    }
    
    
    public override Task Handle(UserModelDto dto, IWebSocketConnection socket)
    {
        var message = new UserModel
        {
            username = dto.username,
            password = dto.password,
            address = dto.address,
            zipcode = dto.zipcode,
            cvr = dto.cvr
        };

        _UserService.CreateUser(message);
        
        
        
        return Task.CompletedTask;
    }
    
    }

public class UserModelDto : BaseDto
{
    
    public string username { get; set; }
    public string password { get; set; }
    public string address { get; set; }
    public int zipcode { get; set; }
    public int? cvr { get; set; }
}










