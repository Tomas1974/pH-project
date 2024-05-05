using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;

namespace ws;




public class UserActions : BaseEventHandler<LoggedInInfo>
{

    public readonly UserService _userService;  
    
    public UserActions(UserService userService) 
    {
        _userService = userService;  
    }
    
    
    
    public override  Task Handle(LoggedInInfo dto, IWebSocketConnection socket)
    {


        Console.WriteLine(dto.getLoginInfo);
        
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
            
           email =    _userService.loginEmail
        
      
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









