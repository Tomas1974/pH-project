using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using lib;

namespace ws;




public class getAddresses(HttpClientService httpService) : BaseEventHandler<getAddressesDto>
{

    
    
    public override async Task Handle(getAddressesDto dto, IWebSocketConnection socket)
    {

        Console.WriteLine("Hej"+dto.addressSearchTerm);
        var message = new sendAddresses()
        {
            
            results= await httpService.GetAddressSuggestion(dto.addressSearchTerm)
        
      
        };
 
        var messageToClient = JsonSerializer.Serialize(message);
        socket.Send(messageToClient);
        
        }

       
    }
    
 



public class getAddressesDto : BaseDto
{
    
    public string addressSearchTerm { get; set; }
}




public class sendAddresses : BaseDto
{
   
    public AddressRootObject results { get; set; }
    
 
}








