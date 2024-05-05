using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;

namespace ws;



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
        
        Console.WriteLine("ddfffd"+userModel.address);
        
        
       var messageToClient = JsonSerializer.Serialize(userModel);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;    
   
   
    }
   
 }


public class getUserInfoDto : BaseDto
{
    
    public string email { get; set; }
     
    
}















