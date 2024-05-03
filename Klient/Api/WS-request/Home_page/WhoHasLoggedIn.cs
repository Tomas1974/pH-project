using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;

namespace ws;




public class WhoHasLoggedIn : BaseEventHandler<LoggedInInfo>
{

    public readonly UserService _userService;  
    
    public WhoHasLoggedIn(UserService userService) 
    {
        _userService = userService;  
    }
    
    
    
    public override  Task Handle(LoggedInInfo dto, IWebSocketConnection socket)
    {

        Console.WriteLine("Test "+dto.getLoginInfo);
        
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









