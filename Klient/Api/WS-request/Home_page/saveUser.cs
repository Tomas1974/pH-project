using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;


namespace ws;


public class saveUser : BaseEventHandler<saveUserDto>
{
    public readonly UserService _UserService;
    
    public saveUser(UserService userService)
    {
        _UserService = userService;
    }
    
    
    public override Task Handle(saveUserDto dto, IWebSocketConnection socket)
    {
        Console.WriteLine("ZipZin "+dto.zip_code);
        var message = new UserModel
        {
            email = dto.email,
            name = dto.name,
            password = dto.password,
            address = dto.address,
            zip_code = dto.zip_code,
            cvr = dto.cvr
        };
        
        string message1=_UserService.CreateUser(message);
        
        
        
        var message2 = new responseString
        {
            
            response = message1
        
      
        };
 
        var messageToClient = JsonSerializer.Serialize(message2);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;    

    }
    
    }

public class saveUserDto : BaseDto
{
    public string email { get; set; }
    public string name { get; set; }
    public string password { get; set; }
    public string address { get; set; }
    public int zip_code { get; set; }
    public int? cvr { get; set; }
}










