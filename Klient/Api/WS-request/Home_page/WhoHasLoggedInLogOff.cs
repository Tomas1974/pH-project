using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;

namespace ws;




public class WhoHasLoggedInLogOff : BaseEventHandler<LoggedInInfo>
{

    public readonly UserService _userService;  
    
    public WhoHasLoggedInLogOff(UserService userService) 
    {
        _userService = userService;  
    }
    
    
    
    public override  Task Handle(LoggedInInfo dto, IWebSocketConnection socket)
    {


        if (dto.getLoginInfo.Contains("logOff")  )
        {
            _userService.loginEmail = "";
            Console.WriteLine("Test");
            
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









