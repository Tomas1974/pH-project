using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.Json;
using api;
using Fleck;
using infrastructure.DataModels;
using lib;
using Service1;

namespace ws;




public class getData : BaseEventHandler<clientIdDto>
{

    public readonly DataService _dataService;  
    
    public getData(DataService dataService) 
    {
        _dataService = dataService;  
    }
    
    
    
    public override  Task Handle(clientIdDto dto, IWebSocketConnection socket)
    {
       
        Console.WriteLine("ddd"+ _dataService.getData(dto.clientID).Count);    
        
        PhData phData = new PhData
        {
            series = _dataService.getData(dto.clientID)

        };

        
        var messageToClient = JsonSerializer.Serialize(phData);
        socket.Send(messageToClient);
        
        return Task.CompletedTask;    
    
    
   
    }
    
 
}


public class clientIdDto : BaseDto
{
    
    public string clientID { get; set; }
     
    
}




public class PhData : BaseDto
{
   
    public List<SeriesData> series { get; set; }
    
 
}











