using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using lib;

namespace ws;




public class loginUser(HttpClientService httpService) : BaseEventHandler<loginUserDto>
{

    
    
    public override  Task Handle(loginUserDto dto, IWebSocketConnection socket)
    {
        
        
        

       // Console.WriteLine("Hej"+dto.addressSearchTerm);
        // var message = new sendAddresses()
        // {
        //     
        //     results= await httpService.GetAddressSuggestion(dto.addressSearchTerm)
        //
        //
        // };
 
        // var messageToClient = JsonSerializer.Serialize(message);
        // socket.Send(messageToClient);
        return Task.CompletedTask;
        }

   
    }
    
 



public class loginUserDto : BaseDto
{
    
    public string username { get; set; }
     public string password { get; set; }
    
}



//
// public class sendAddresses : BaseDto
// {
//    
//     public AddressRootObject results { get; set; }
//     
//  
// }








