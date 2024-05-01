using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Fleck;
using lib;

using Websocket;

namespace ws;


public class StartStop : BaseEventHandler<StartStopDto>
{

   
    
    public override Task Handle(StartStopDto dto, IWebSocketConnection socket)
    {
    
        Console.WriteLine("Hej");
        
        return Task.CompletedTask;
    }
    
    }

public class StartStopDto : BaseDto
{
    
    public string start_stop { get; set; }
    
}










